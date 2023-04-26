using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MemoryGame_FinalProject
{
    //for celltypes, we need a 'blank' (or uncovered) card, and a bunch of different uncovered card types
    //we also need a type for when the player has correctly matched 2 cards and they are 'discarded'
    enum CellTypes { blank = 0, card1 = 1, card2 = 2, card3 = 3, card4 = 4, discard = 5 }
    public partial class Form1 : Form
    {

        //first, we need to make the board and cell sizes
        Size gridSize = new Size(8, 8); //how many "cards" there are, u can change thru menu strip (do this later)
        Size cellSize = new Size(50, 50); //how many pixels each "card" is wide/high

        //the number we will start to set cells to
        int number = 1;

        //the int counting how many cells have already been set to that number
        int counter = 0;

        //dont know that i'll be able to do this...
        //we want to make a varible for the 'difficulty'
        //16 = easy, 

        //graphics!!
        Bitmap graphics = new Bitmap(typeof(Form1), "Memory Graphics.png");

        //make our 2d array
        CellTypes[,] gameData;

        //the "layer" that covers all cells
        bool[,] covered;

        //the bool array/"layer" the will determine whetehr or not cells are matched
        bool[,] matched;

        //the coordinate of the last clicked
        //at the start of the game we will set this variable to be off the board
        Point firstClick = new Point(-1, -1);
        Point secondClick = new Point(-1, -1);


        public Form1()
        {
            InitializeComponent();
            setUp();
            setCards();
        }

        //for the first start of the game (when we need to set up the board)
        private void setUp()
        {
            ClientSize = new Size(gridSize.Width * cellSize.Width, gridSize.Height * cellSize.Height + menuStrip1.Height);
            gameData = new CellTypes[gridSize.Width, gridSize.Height];
            covered = new bool[gridSize.Width, gridSize.Height];
            matched = new bool[gridSize.Width, gridSize.Height];

            coverCells();

            Invalidate();
        }

        private void coverCells()
        {
            //loop through the board to set all cells to covered in the beginning of a new game
            //we will also set all cells to unmatched (because we'll be calling this function at the beginning of a new game)
            for (int i = 0; i < gridSize.Width; i++)
            {
                for (int j = 0; j < gridSize.Height; j++)
                {
                    covered[i, j] = true;
                    matched[i, j] = false;
                }
            }

        }

        //set every BLANK cell to a card        
        private void setCards()
        {
            //keep cards in pairs (set at least 2 cells to the same card so there's a 'pair')
            //set each cell to a random number between 1 and 8, then set numbers to a different colour in paint function
            //how to make sure there's an even number of each number?
            //loop through the whole board to start

            //a random number variable for assigning numbers to cards
            Random rand = new Random();

            //random variables for every cell of the board
            //this will enable a cell to be randomly selected

            for (int i = 0; i < gridSize.Width * gridSize.Height; i++)
            {

                int x = rand.Next(0, gridSize.Width);
                int y = rand.Next(0, gridSize.Height);

                //only do this IF the cell has not already been set (it's a 0)
                if (gameData[x, y] == 0)
                {
                    //set it to the current number
                    //(we need a cast for this to work)
                    gameData[x, y] = (CellTypes)number;
                    //up the counter
                    counter++;
                }

                else
                    i--;

                if (counter == 16)
                {
                    //up the number
                    number++;
                    //set the counter back to 0
                    counter = 0;
                }

            }

            Invalidate();
        }

        //for painting all graphics in the game
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //we start by making the board

            //adjust all graphics down by the height of the menustrip
            e.Graphics.TranslateTransform(0, menuStrip1.Height);

            //our brush, for painting the board
            Brush a = new SolidBrush(Color.GreenYellow);
            Brush b = new SolidBrush(Color.Aqua);
            Brush c = new SolidBrush(Color.Beige);
            Brush d = new SolidBrush(Color.Red);
            Brush coveredBrush = new SolidBrush(Color.Silver);
            Brush disabledBrush = new SolidBrush(Color.DarkSlateGray);

            //a double for loop, to go through the whole board and paint it
            for (int i = 0; i < gridSize.Width; i++)
            {
                for (int j = 0; j < gridSize.Height; j++)
                {
                    Rectangle r = new Rectangle(i * cellSize.Width, j * cellSize.Height, cellSize.Width, cellSize.Height);

                    //put some "if" loops in here until we can do it more efficiently
                    //set each cell to a specific colour based on the number it's been assigned

                    //bool cellCovered = false;

                    //we need 2 rectangles, the image to choose from the graphic strip and where to draw it on the game board
                    Rectangle srcRect = new Rectangle();

                    //where to draw the image on the game board
                    Rectangle destRect = new Rectangle(i * cellSize.Width, j * cellSize.Height, cellSize.Width, cellSize.Height);

                    if (covered[i, j])
                    {
                        e.Graphics.FillRectangle(coveredBrush, r); //fill the covered cells 
                    }

                    //if the cell is matched, we want to set it to a disabled type color
                    if (matched[i, j])
                    {
                        e.Graphics.FillRectangle(disabledBrush, r);
                    }

                    if (covered[i, j] == false)
                    {

                        if (matched[i, j] == false)
                        {
                            //if that specific cell has been set to a 1
                            if (gameData[i, j] == CellTypes.card1)
                            {
                                srcRect = new Rectangle(0, 0, cellSize.Width, cellSize.Height);
                                e.Graphics.DrawImage(graphics, destRect, srcRect, GraphicsUnit.Pixel);
                            }

                            else if (gameData[i, j] == CellTypes.card2)
                            {
                                srcRect = new Rectangle(2 * cellSize.Width, 0, cellSize.Width, cellSize.Height);
                                e.Graphics.DrawImage(graphics, destRect, srcRect, GraphicsUnit.Pixel);
                            }

                            else if (gameData[i, j] == CellTypes.card3)
                            {
                                srcRect = new Rectangle(3 * cellSize.Width, 0, cellSize.Width, cellSize.Height);
                                e.Graphics.DrawImage(graphics, destRect, srcRect, GraphicsUnit.Pixel);
                            }

                            else if (gameData[i, j] == CellTypes.card4)
                            {
                                srcRect = new Rectangle(1 * cellSize.Width, 0, cellSize.Width, cellSize.Height);
                                e.Graphics.DrawImage(graphics, destRect, srcRect, GraphicsUnit.Pixel);
                            }

                        }
                    }

                }
            }

            //draw the gridlines, working across the board
            for (int i = 0; i < gridSize.Width; i++)
            {
                //horizontal lines
                e.Graphics.DrawLine(Pens.Black, 0, i * cellSize.Height, ClientRectangle.Right, i * cellSize.Height);

                //vertical lines
                e.Graphics.DrawLine(Pens.Black, i * cellSize.Width, 0, i * cellSize.Width, ClientRectangle.Bottom);
            }
        }

        //now i think i have to work on the mouse down function
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            //for this, we first have to identify which cell was clicked
            //then uncover it to reveal the colour

            Point cellSelected = new Point(e.X / cellSize.Width, (e.Y - menuStrip1.Height) / cellSize.Height);

            //make sure our click is within the boundaries of the grid
            if (cellSelected.X < 0 || cellSelected.X >= gridSize.Width || cellSelected.Y < 0 || cellSelected.Y >= gridSize.Height)
            {
                return; //the rest of the code of this function is skipped
            }

            //we only want to be able to click 2 cells, if they're not already matched or uncovered 
            if (!matched[cellSelected.X, cellSelected.Y])
            {
                if (covered[cellSelected.X, cellSelected.Y])
                {
                    //if the timer isn't on, then do this
                    if (timerTurn.Enabled == false)
                    {

                        //if it's the first click of the game/the pair
                        if (firstClick.X == -1)
                        {
                            //set last placed to the click we just made
                            firstClick = new Point(cellSelected.X, cellSelected.Y);

                            //uncover it
                            covered[cellSelected.X, cellSelected.Y] = false;

                            Invalidate();
                        }

                        else
                        {
                            //its now the second click
                            secondClick = new Point(cellSelected.X, cellSelected.Y);

                            covered[secondClick.X, secondClick.Y] = false;

                            //show the uncovered cell before anything else happens
                            Invalidate();

                            //see if they're the same card
                            if (gameData[firstClick.X, firstClick.Y] == gameData[secondClick.X, secondClick.Y])
                            {
                                //we have a match!
                                MessageBox.Show("congrats there's a match");

                                //set both spots on the board to 'matched'
                                matched[firstClick.X, firstClick.Y] = true;
                                matched[secondClick.X, secondClick.Y] = true;
                            }

                            //we don't have a match :(
                            else
                            {
                                timerTurn.Enabled = true;
                                //start a timer
                                timerTurn.Start();
                            }

                        }

                    }
                }
            }

            //set the first and second click back to -1 every time a click occurs, we don't want to be using old data:)
            if (firstClick.X > -1 && secondClick.X > -1)
            {
                firstClick = new Point(-1, -1);
                secondClick = new Point(-1, -1);
            }

            Invalidate();
        }

        #region MenuStrip stuff

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set number variable back to their originals so it won't skip over the function
            number = 1;

            //make a double for loop to set every single card back to blank, otherwise it will skip over the whole set cards function
            for (int i = 0; i < gridSize.Width; i++)
            {
                for (int j = 0; j < gridSize.Height; j++)
                {
                    gameData[i, j] = CellTypes.blank;
                }
            }

            firstClick = new Point(-1, -1);
            secondClick = new Point(-1, -1);

            coverCells();

            setCards();

            Invalidate();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult d = MessageBox.Show("Are you sure you'd like to quit?", "Exit Game", MessageBoxButtons.YesNo);

            if (d == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void timerTurn_Tick(object sender, EventArgs e)
        {
            //when this ticks, scan the board

            //if it's uncovered but not part of a match, do this
            for (int i = 0; i < gridSize.Width; i++)
            {
                for (int j = 0; j < gridSize.Height; j++)
                {
                    if (!matched[i, j])
                    {
                        //cover them back up
                        covered[i, j] = true;
                        Invalidate();
                    }

                }
            }

            timerTurn.Stop();
            timerTurn.Enabled = false;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Kailey Kellett, 2019\nA simple memory game, with graphics made by yours truly.", "About the Game");
        }

        #endregion
    }
}
