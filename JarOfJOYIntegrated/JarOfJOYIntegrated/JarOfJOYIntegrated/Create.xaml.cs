using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JarOfJOYIntegrated
{
    public partial class Create : PhoneApplicationPage
    {
        /* VARIABLES */
        
        // Initialize variables
        ContentManager contentManager;
        GameTimer timer;
        SharedGraphicsDeviceManager graphics;

        // Make spriteBatch of tiles
        SpriteBatch spriteBatch;
        Texture2D myTexture;
        Vector2 spritePosition = new Vector2(0.0f, 0.0f);
        Vector2 spriteSpeed = new Vector2(50.0f, 50.0f);
        SpriteFont Font1;

        // Make spriteBatch for next button
        SpriteBatch nextBatch;
        Texture2D nextTexture;
        Vector2 nextPosition = new Vector2(410.0f, 730.0f);
        Vector2 nextSpeed = new Vector2(0.0f, 0.0f);
        
        // Establish size of icon
        public static int ICON = 60;

        // Make spriteBatch for prev button
        SpriteBatch prevBatch;
        Texture2D prevTexture;
        Vector2 prevPosition = new Vector2(10.0f, 730.0f);
        Vector2 prevSpeed = new Vector2(0.0f, 0.0f);

        // Make spriteBatch for number page
        SpriteBatch currBatch;
        Texture2D currTexture;
        Vector2 currPosition = new Vector2(200.0f, 740.0f);
        Vector2 currSpeed = new Vector2(0.0f, 0.0f);

        // Make spriteBatch for current text
        SpriteBatch textBatch;
        Texture2D textTexture;
        Vector2 textPosition = new Vector2(6.0f, 30.0f);
        Vector2 textTextPosition = new Vector2(20.0f, 40.0f);
        Vector2 textSpeed = new Vector2(0.0f, 0.0f);
        
        // Establish empty string for current text
        public static string textText = "";

        // Make spriteBatch for target area
        SpriteBatch targetBatch;
        Texture2D targetTexture;
        Vector2 targetPosition = new Vector2(6.0f, 150.0f);
        Vector2 targetTextPosition = new Vector2(16.0f, 160.0f);
        Vector2 targetSpeed = new Vector2(0.0f, 0.0f);
        
        // Establish empty string for target text
        public static string targetText = "";

        // Make spriteBatch for yes
        SpriteBatch yesBatch;
        Texture2D yesTexture;
        Vector2 yesPosition = new Vector2(405.0f, 180.0f);
        Vector2 yesSpeed = new Vector2(0.0f, 0.0f);

        // Make spriteBatch for no
        SpriteBatch noBatch;
        Texture2D noTexture;
        Vector2 noPosition = new Vector2(405.0f, 270.0f);
        Vector2 noSpeed = new Vector2(0.0f, 0.0f);

        // Global Constant for number of words for tiles
        public const int NUMWORDS = 180;

        // Global Constant for number of words per page
        public const int NUMWORDSPAGE = 18;

        // Global variable for current page of create tiles
        public static int CURRPAGE = 0;

        // Global Constants for width and height of create tiles
        public const int TWIDTH = 156;
        public const int THEIGHT = 58;

        // Global variable for whether or not tile is clicked
        public static Boolean currTile;

        // Array of words
        public string[] wordList = new string[NUMWORDS];

        // Array of words to put on tiles		
        Tile[] Piece = new Tile[NUMWORDS];
        string wordListText;

        // Global variable for whether or not create page is in original, unedited position
        public static Boolean unedited;

        // Global variable for original Tiles, unedited
        Tile[] original = new Tile[NUMWORDS];

        // Global variable to keep track of last clicked tile
        public static int lastIndex;

        // Global variable to keep track of which tiles were deleted
        public static bool deleted = false;

        /* CONSTRUCTOR */

        // Draws the UI
        public Create()
        {
            InitializeComponent();

            // Get the content manager from the application
            contentManager = (Application.Current as App).Content;

            // Create a timer for this page
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;

            // Configuration and management of shared graphics device
            graphics = SharedGraphicsDeviceManager.Current;
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
        }

        /* ON MOTION */
        
        // When the page is loaded
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Set the sharing mode of the graphics device to turn on XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // Create a new SpriteBatch for tiles
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            myTexture = contentManager.Load<Texture2D>("Tile");
            Font1 = contentManager.Load<SpriteFont>("SpriteFont1");

            // Create a new SpriteBatch for next button
            nextBatch = new SpriteBatch(graphics.GraphicsDevice);
            nextTexture = contentManager.Load<Texture2D>("Next");

            // Create a new SpriteBatch for previous button
            prevBatch = new SpriteBatch(graphics.GraphicsDevice);
            prevTexture = contentManager.Load<Texture2D>("Prev");

            // Create a new SpriteBatch for current page
            currBatch = new SpriteBatch(graphics.GraphicsDevice);
            currTexture = contentManager.Load<Texture2D>("Curr");

            // Create a new SpriteBatch for current text
            textBatch = new SpriteBatch(graphics.GraphicsDevice);
            textTexture = contentManager.Load<Texture2D>("Text");

            // Create a new SpriteBatch for target area
            targetBatch = new SpriteBatch(graphics.GraphicsDevice);
            targetTexture = contentManager.Load<Texture2D>("Target");

            // Create a new SpriteBatch for yes button
            yesBatch = new SpriteBatch(graphics.GraphicsDevice);
            yesTexture = contentManager.Load<Texture2D>("Yes");

            // Create a new SpriteBatch for no button
            noBatch = new SpriteBatch(graphics.GraphicsDevice);
            noTexture = contentManager.Load<Texture2D>("No");

            try
            {
                // Initialize the array of tiles
                for (int p = 0; p < NUMWORDS; p++)
                {
                    Piece[p] = new Tile();
                    original[p] = new Tile();
                }

                // Create a string for the words
                string wordListText1 = LoadWords();

                // Break the string into an array of characters
                char[] s = wordListText1.ToCharArray();

                // Initialize length of string, temporary string, and counter
                int length = s.Length;
                string temp = "";
                int j = 0;

                // Parse the words separated by new line characters into the array
                for (int i = 0; i < length; i++)
                {
                    // If not a new line character, continue adding to string
                    if (s[i] != '\n')
                        temp += s[i];
                    else
                    {
                        // Or else simply add to the word list
                        wordList[j] = temp;
                        
                        // Reinitialize temporary variable and increment counter
                        temp = "";
                        j++;
                    }

                }

                // These are the ultimate x and y cordinates
                float xcord = 0;
                float ycord = 0;

                // Leeps track of which row the set of tiles is in
                int row = 0;
                int page = 0;
                int index = 0;

                // Size of the tile in x and y dimensions
                int tilex = TWIDTH;
                int tiley = THEIGHT;

                // Size of canvas in y dimension
                int canvasy = 364;

                // Size of buffer between tiles in x and y dimensions
                int bufferx = 3;
                int buffery = 2;

                // Create each tile in 3 x 6 grid based on index in array
                for (int k = 0; k < 10; k++)
                {
                    // Looping through each of the 18 words on a page
                    for (int i = 0; i < NUMWORDSPAGE; i++)
                    {
                        // Places in first column
                        if ((i % 3) == 0)
                        {
                            // Buffer away from left side
                            xcord = bufferx;
                            // Account for text/target sprites and row
                            ycord = canvasy + 3 + row * (tiley + buffery);
                        }
                        // Places in second column
                        else if ((i % 3) == 1)
                        {
                            xcord = bufferx + tilex + bufferx;
                            ycord = canvasy + 3 + row * (tiley + buffery);
                        }
                        // Places in third column
                        else
                        {
                            xcord = bufferx + tilex + bufferx + tilex + bufferx;
                            ycord = canvasy + 3 + row * (tiley + buffery);
                            
                            // Increase row counter
                            row++;
                        }

                        // Establish word on the tile
                        Piece[index].Word = wordList[index];
                        
                        // Establish position of the tile
                        Piece[index].spritePosition = new Vector2(xcord, ycord);
                        
                        // Establish position of the word
                        Piece[index].fontPosition.X = Piece[index].spritePosition.X + 10;
                        Piece[index].fontPosition.Y = Piece[index].spritePosition.Y + 10;
                        
                        // Establish location of tile
                        Piece[index].spriteRow = row;
                        Piece[index].spritePage = page;

                        // Initialize an array that keeps track of original properties of tile
                        original[index].Word = wordList[index];
                        original[index].spritePosition = new Vector2(xcord, ycord);
                        original[index].fontPosition.X = original[index].spritePosition.X + 10;
                        original[index].fontPosition.Y = original[index].spritePosition.Y + 10;
                        original[index].spriteRow = row;
                        original[index].spritePage = page;

                        // Increase counter
                        index++;
                    }

                    // Increase page
                    page++;

                    // Reinitialize row counter
                    row = 0;
                }

                // Reinitialized unedited
                unedited = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
            // Load the words
            Load();

            // Start the timer
            timer.Start();

            base.OnNavigatedTo(e);
        }

        // When user leaves the page
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Stop the timer
            timer.Stop();

            // Set the sharing mode of the graphics device to turn off XNA rendering
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

            base.OnNavigatedFrom(e);
        }

        // Allows the page to run logic such as updating the world, checking for collisions, gathering input, and playing audio.
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {

            //TOUCH
            // Process touch events
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                // when there is a touch on the screen
                if ((tl.State == TouchLocationState.Pressed))
                {
                    // loop through each tile
                    for (int i = 0; i < NUMWORDS; i++)
                    {
                        // only enable tiles on current page
                        if (Piece[i].spritePage == CURRPAGE)
                        {
                            
                            // if nothing is already clicked
                            if (!currTile)
                            {
                                // activate tile user touched if needed
                                if (tl.Position.X < Piece[i].spritePosition.X + TWIDTH
                                    && tl.Position.X > Piece[i].spritePosition.X - 5
                                    && tl.Position.Y < Piece[i].spritePosition.Y + 5
                                    && tl.Position.Y > Piece[i].spritePosition.Y - THEIGHT)
                                {
                                    Piece[i].isClicked = true;
                                    currTile = true;
                                    deleted = false;
                                    lastIndex = i;
                                }

                                // if touching near yes button, return tile
                                if (tl.Position.X < yesPosition.X + ICON
                                    && tl.Position.X > yesPosition.X - 10
                                    && tl.Position.Y < yesPosition.Y + 20
                                    && tl.Position.Y > yesPosition.Y - ICON)
                                {
                                    MoveSprite(original[i].spritePosition.X, original[i].spritePosition.Y, i);
                                    Piece[i].isClicked = false;
                                }

                            }
                            // if tile is activated
                            else if (Piece[i].isClicked == true)
                            {
                                if (tl.Position.X < 480
                                    && tl.Position.X > 0
                                    && tl.Position.Y > 110
                                    && tl.Position.Y < 310)
                                {
                                    // move tile to new touch location
                                    MoveSprite(tl.Position.X, tl.Position.Y, i);

                                    // add text to textText
                                    AddText(i);

                                    //MoveSprite(x, y, i);

                                    // unactivate tile
                                    Piece[i].isClicked = false;

                                    // no tile clicked anymore
                                    currTile = false;

                                    // remember index of last tile clicked
                                    lastIndex = i;
                                    deleted = false;
                                }
                                //else
                                    //Piece[i].isClicked = false;

                                
                            }
                        }
                    }

                    //if touching near no button
                    if (tl.Position.X < noPosition.X + ICON
                        && tl.Position.X > noPosition.X - 10
                        && tl.Position.Y < noPosition.Y + 20
                        && tl.Position.Y > noPosition.Y - ICON
                        && (deleted == false))
                    {
                        // delete last word added
                        RemoveText(lastIndex);
                        // return tile to grid
                        MoveSprite(original[lastIndex].spritePosition.X, original[lastIndex].spritePosition.Y, lastIndex);
                        Piece[lastIndex].isClicked = false;
                        Debug.WriteLine("i" + lastIndex);
                        deleted = true;
                    }

                    // if no tile was originally clicked
                    if (!currTile)
                    {
                        // if touching near previous button, reduce page of tiles
                        if (tl.Position.X < prevPosition.X + ICON
                            && tl.Position.X > prevPosition.X - 10
                            && tl.Position.Y < prevPosition.Y + 10
                            && tl.Position.Y > prevPosition.Y - ICON
                            && CURRPAGE > 0)
                            CURRPAGE--;
                        
                        // if touching near next button, increase page of tiles
                        if (tl.Position.X < nextPosition.X + ICON
                            && tl.Position.X > nextPosition.X - 10
                            && tl.Position.Y < nextPosition.Y + 10
                            && tl.Position.Y > nextPosition.Y - ICON
                            && CURRPAGE < 9)
                            CURRPAGE++;
                    }
                }
            }
        }

        // Allows the page to draw itself.
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            //SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.GraphicsDevice.Clear(Color.Black);

            // Draws previous button, current page and next button
            currBatch.Begin();
            nextBatch.Begin();
            prevBatch.Begin();
            
            // initialize counter for page
            string currPage = "";
            currPage = (CURRPAGE + 1) + " of 10";

            // draw counter for current page
            currBatch.Draw(currTexture, currPosition, Color.Black);
            currBatch.DrawString(Font1, currPage, currPosition, Color.White);

            // draw the previous page button if not first page
            if (CURRPAGE != 0)
                prevBatch.Draw(prevTexture, prevPosition, Color.White);

            // draw the next page button if not last page
            if (CURRPAGE != 9)
                nextBatch.Draw(nextTexture, nextPosition, Color.White);

            // Ends the drawing
            currBatch.End();
            nextBatch.End();
            prevBatch.End();

            // Draws textBatch, sprite for displaying current composition
            textBatch.Begin();
            textBatch.Draw(textTexture, textPosition, Color.White);
            textBatch.DrawString(Font1, textText, textTextPosition, Color.Black);
            textBatch.End();

            // Draws targetBatch, sprite for target area to drop tiles into for adding to composition
            targetBatch.Begin();
            targetBatch.Draw(targetTexture, targetPosition, Color.White);
            
            // If the screen is unedited, include instructional text
            if (unedited)
                targetText = "\n\n            Click the tiles and tap here\n            to append it to the quote";
            
            // Clear instructional text
            else
            {
                targetText = "";
            }
            
            // Draw text
            targetBatch.DrawString(Font1, targetText, targetTextPosition, Color.White);
            targetBatch.End();

            // Draws yesBatch
            yesBatch.Begin();
            yesBatch.Draw(yesTexture, yesPosition, Color.White);
            yesBatch.End();

            // Draws noBatch
            noBatch.Begin();
            noBatch.Draw(noTexture, noPosition, Color.White);
            noBatch.End();

            // Draws tile sprites
            spriteBatch.Begin();
            Color fontColor;

            // Loop through all tiles
            for (int i = 0; i < NUMWORDS; i++)
            {
                // Looking only at those on the current page
                if (Piece[i].spritePage == CURRPAGE)
                {
                    // Draw the sprite texture
                    spriteBatch.Draw(myTexture, Piece[i].spritePosition, Color.White);

                    // Change the font color from black to red if clicked
                    if (Piece[i].isClicked)
                        fontColor = Color.Red;
                    else
                    {
                        fontColor = Color.Black;
                    }

                    // Draw the string for the sprite
                    spriteBatch.DrawString(Font1, Piece[i].Word, Piece[i].fontPosition, fontColor);
                }
            }
            spriteBatch.End();
        }

        /* LOADING WORDS */
        
        // Fetches words from xml file and returns it as a string
        public string LoadWords()
        {

            // Use XmlReader to get words individually from xml file
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create("words.xml"))
            {
                reader.MoveToContent();
                reader.ReadToFollowing("wordList");
                wordListText = reader.ReadInnerXml();
            }

            // Return the string of words
            return wordListText;

        }

        // Takes the loaded words and places them in an array to load the tiles
        protected void Load()
        {
            // Reinitialize text to original screen
            textText = "Your quote will appear here";
            targetText = "\n\n            Click the tiles and tap here\n            to append it to the quote";
            
            // Reinitialize properties of tile to original
            for (int index = 0; index < NUMWORDS; index++)
            {
                Piece[index].Word = original[index].Word;
                Piece[index].spritePosition = original[index].spritePosition;
                Piece[index].fontPosition = original[index].fontPosition;
                Piece[index].spriteRow = original[index].spriteRow;
                Piece[index].spritePage = original[index].spritePage;
            }
 
            // Reinitialize unedited to be true
            unedited = true;

        }

        /* MOVING AND EDITING TEXT */

        // Changes sprite position to that of new click
        protected void MoveSprite(float x, float y, int index)
        {
            Piece[index].spritePosition.X = x;
            Piece[index].spritePosition.Y = y;

            Piece[index].fontPosition.X = x + 10;
            Piece[index].fontPosition.Y = y + 10;
        }

        // Appends word on moved tile to textText string
        protected void AddText(int i)
        {
            // Clear instructional text if necessary
            if (unedited == true)
            {
                textText = "";
                unedited = false;
            }
            
            // Add tile text to end of text string
            textText += Piece[i].Word + " ";
        }

        // Remove last placed tile
        protected void RemoveText(int i)
        {
            // Cannot clear on empty screen
            if (unedited == true)
                return;
            
            // Calculate length of word and of entire string
            int strlen = Piece[i].Word.Length;
            int textlen = textText.Length;
            
            // Leave only original text minus latest added text
            if (textlen > strlen)
                textText = textText.Remove(textlen - strlen - 1);
        }
        
        /* EVENT HANDLERS */

        // Application navigation bar, brings user to CREATE
        private void Create_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Create.xaml", UriKind.Relative));
        }

        // Application navigation bar, brings user to RANDOM
        private void Random_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/RandomQuote.xaml", UriKind.Relative));
        }

        // Application navigation bar, brings user to HOME
        private void Home_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Home.xaml", UriKind.Relative));
        }

        // Application navigation bar, brings user to INFO
        private void Info_Clicked(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info.xaml", UriKind.Relative));
        }

        // Menu item, brings user to help menu for create
        private void Info_Create(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Info_Create.xaml", UriKind.Relative));
        }
        
        // Menu item, clears entire string of text
        private void Clear_Page(object sender, EventArgs e)
        {
            Load();
        }

        // Menu item, saves entire string of text for future access
        private void Save_Page(object sender, EventArgs e)
        {
            App.Apple.addedQuote = true;
            App.Apple.quoteList.Add(textText);
            Load();
        }

        
    }
}