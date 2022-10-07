
// global input for all the files

global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using Microsoft.Xna.Framework.Audio;
global using System.Collections.Generic;
global using System;
using System.IO;



namespace Game_2048;

public class Game1 : Game
{
    public GraphicsDeviceManager _graphics;
    public SpriteBatch _spriteBatch;
    Basicfunc basf;
    Texture2D block_texture;


    // declaring some constants
    public const int size = 150;
    public const int max_r_c = 4;  // 4 * 4
    public const int gap = 10;
    public const int speed = 30;


    // initial_x and initial_y for the box
    int initial_x = 10;
    int initial_y = 10;

    // block array's to work with the block's on the screen
    List<Block> normal_blocks;
    List<Block> changing_blocks;


    int total_size;
    int speed_x, speed_y;
    int score;
    string high_score;


    bool is_to_perform_transitioning;
    bool is_to_rearrange;
    bool is_button_pressed;




    Random rand;
    SpriteFont sfont;




    Direction direction = Direction.Right;

    Color[] colors;









    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        this.basf = new Basicfunc(this, 850, 700, false);
        this.basf.settitle("Block 2048");
        this.rand = new Random();
        this.speed_x = 0;
        this.colors = new Color[12] { Color.DarkBlue, Color.DarkCyan, Color.DarkGray, Color.DarkMagenta, Color.DarkOrange, Color.DarkOrchid, Color.DarkKhaki, Color.Pink, Color.BlueViolet, Color.Thistle, Color.Tomato, Color.Turquoise };


        this.score = 0;
        this.high_score = File.ReadAllLines("data.txt")[0];



        Content.RootDirectory = "Content";
        IsMouseVisible = true;


        this.is_to_perform_transitioning = false;
        this.is_to_rearrange = false;
        this.is_button_pressed = false;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        this.sfont = this.Content.Load<SpriteFont>("font");


        var con_x = initial_x;
        var con_y = initial_y;


        normal_blocks = new List<Block>();
        changing_blocks = new List<Block>();

        reset_the_blocks();

        // File.WriteAllLines("data.txt",new String[1]{$"3999"});


        base.Initialize();
    }





    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // loading the block texture
        // this texture is use to display the different type's of color
        block_texture = this.Content.Load<Texture2D>("solidcolor");


        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        var ks = Keyboard.GetState();



        // setting the direction with the use input
        if (!is_to_perform_transitioning)
        {
            if (ks.IsKeyDown(Keys.D))
            {
                set_move(speed, 0, Direction.Right);
            }
            else if (ks.IsKeyDown(Keys.A))
            {
                set_move(-speed, 0, Direction.left);

            }

            else if (ks.IsKeyDown(Keys.W))
            {
                set_move(0, -speed, Direction.Up);

            }

            else if (ks.IsKeyDown(Keys.S))
            {
                set_move(0, speed, Direction.Down);

            }
            else
            {
                is_button_pressed = false;
            }
        }



        // using the direction and performing the motion of the blocks or boxes
        if (is_to_perform_transitioning)
        {

            foreach (var item in changing_blocks)
            {
                // working with the direction and as per the direction performing the motion of the boxes
                if (direction == Direction.Right)
                {
                    perform_transition(item, item.x + size + gap < total_size + initial_x && item.is_transitioning, "horizontal", "right");
                }
                else if (direction == Direction.left)
                {
                    perform_transition(item, item.x > initial_x + gap && item.is_transitioning, "horizontal", "left");
                }
                else if (direction == Direction.Up)
                {
                    perform_transition(item, item.y > initial_y + gap && item.is_transitioning, "vertical", "up");
                }
                else if (direction == Direction.Down)
                {
                    perform_transition(item, item.y + size + gap < initial_y + total_size && item.is_transitioning, "vertical", "down");
                }

            }

        }


        // resetting to the original consituent's of the blocks or boxes
        // after the motion or transition of the boxes in been dones
        if (is_to_rearrange)
        {
            is_to_rearrange = false;
            is_to_perform_transitioning = false;
            foreach (var item in changing_blocks)
            {
                item.is_conjugation_performed = false;
            }

            var arr = new List<Block>();
            var free_co_ordinates = new List<Block>();

            arr.Clear();
            free_co_ordinates.Clear();

            foreach (var item in changing_blocks)
            {
                if (!item.is_to_show)
                {
                    arr.Add(item);
                }
            }
            foreach (var item in normal_blocks)
            {
                var is_acquired = false;
                foreach (var item2 in changing_blocks)
                {
                    if (Math.Abs(item.x - item2.x) < 10 && Math.Abs(item.y - item2.y) < 10)
                    {
                        is_acquired = true;
                    }
                }
                if (!is_acquired)
                {
                    free_co_ordinates.Add(item);
                }
            }
            if (free_co_ordinates.Count > 0)
            {

                // creating a new block which will be having the initail value as 2
                var num = rand.Next(0, free_co_ordinates.Count);
                var num2 = rand.Next(0, arr.Count);
                arr[num2].x = free_co_ordinates[num].x;
                arr[num2].y = free_co_ordinates[num].y;
                arr[num2].is_to_show = true;
                arr[num2].value = 2;
            }
            else
            {
                if (score > Convert.ToInt32(high_score))
                {
                    File.WriteAllLines("data.txt", new String[1] { score.ToString() });
                }
                Exit();
            }

        }

        base.Update(gameTime);
    }




    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Gray);


        basf.displayimgrect(block_texture, new Rectangle(initial_x, initial_y, total_size, total_size), Color.Gray);
        basf.drawtext(sfont, "Score:-", new Vector2(initial_x + total_size + 10, initial_y + 100), Color.Black, new Vector2(3, 3));
        basf.drawtext(sfont, score.ToString(), new Vector2(initial_x + total_size + 50, initial_y + 200), Color.Black, new Vector2(3, 3));
        basf.drawtext(sfont, "H-Score:-", new Vector2(initial_x + total_size + 10, initial_y + 300), Color.Black, new Vector2(3, 3));
        basf.drawtext(sfont, high_score, new Vector2(initial_x + total_size + 50, initial_y + 400), Color.Black, new Vector2(3, 3));



        // displaying the normal blocks    :- just for designing
        foreach (var item in normal_blocks)
        {
            basf.displayimgrect(block_texture, new Rectangle(item.x, item.y, size, size));
        }

        foreach (var item in changing_blocks)
        {
            if (item.is_to_show)
            {
                basf.displayimgrect(block_texture, new Rectangle(item.x, item.y, size, size), item.box_color);
                basf.drawtext(sfont, $"{item.value}", new Vector2(item.x + sfont.MeasureString($"{item.value}").X / 2, item.y), Color.Black, new Vector2(4, 4));
            }
        }


        base.Draw(gameTime);

    }

    // this function is used to perform_transition,this function is valid for all the direction.
    public void perform_transition(Block item, bool condition1, string axis, string dir)
    {
        if (item.is_to_show)
        {
            if (condition1)
            {

                var l_x_pos = initial_x + total_size - Math.Abs(size + gap);
                var l_y_pos = initial_y + total_size - Math.Abs(size + gap);

                item.x += speed_x;
                item.y += speed_y;
            }
            else
            {

                s_t_b_t_s(item);

                foreach (var item2 in changing_blocks)
                {
                    if (item.GetHashCode() != item2.GetHashCode())
                    {
                        var condition2 = (axis == "horizontal") ? item2.y == item.y : item2.x == item.x;
                        if (condition2)
                        {
                            var condition3 = false;
                            if (dir == "left")
                            {
                                condition3 = item2.x <= item.x + size + gap;
                            }
                            if (dir == "up")
                            {
                                condition3 = item2.y <= item.y + size + gap;
                            }
                            if (dir == "down")
                            {
                                condition3 = item2.y + size + gap >= item.y;
                            }
                            if (dir == "right")
                            {
                                condition3 = item2.x + size + gap > item.x;
                            }

                            if (condition3)
                            {
                                if (item2.value == item.value && !item.is_conjugation_performed && !item2.is_conjugation_performed && item2.is_transitioning)
                                {
                                    item.value += item2.value;
                                    score += item2.value * 2;
                                    item.box_color = colors[rand.Next(0, colors.Length)];
                                    item2.is_to_show = false;
                                    item.is_conjugation_performed = true;
                                    item2.is_conjugation_performed = true;
                                }

                                s_t_b_t_s(item2);

                            }
                        }
                    }
                }
            }
        }
        foreach (var item2 in changing_blocks)
        {
            if (item2.is_transitioning)
            {
                is_to_rearrange = false;
                break;
            }
            else
            {
                is_to_rearrange = true;
            }
        }
    }


    //s_t_b_t_s  :- short for set_boxes_to_the_standard
    public void s_t_b_t_s(Block item)
    {
        if (item.is_transitioning)
        {
            foreach (var n_box in normal_blocks)
            {
                if (Math.Abs(n_box.x - item.x) < 100 && Math.Abs(n_box.y - item.y) < 100)
                {
                    item.x = n_box.x;
                    item.y = n_box.y;
                    item.is_transitioning = false;
                    break;
                }
            }
        }
    }


    public void set_is_to_move(int speed_x, int speed_y, Direction dir)
    {
        if (!is_to_perform_transitioning)
        {
            this.direction = dir;
            is_to_perform_transitioning = true;
            this.speed_x = speed_x;
            this.speed_y = speed_y;


            foreach (var item in changing_blocks)
            {
                if (item.is_to_show)
                {
                    item.is_transitioning = true;
                }
            }
        }
    }

    public void set_move(int speed_x, int speed_y, Direction dir)
    {
        if (!is_button_pressed)
        {
            set_is_to_move(speed_x, speed_y, dir);
            is_button_pressed = true;
        }
    }

    public void reset_the_blocks()
    {
        var con_x = initial_x;
        var con_y = initial_y;
        changing_blocks.Clear();
        normal_blocks.Clear();
        for (var i = 0; i < max_r_c; i++)
        {

            for (var j = 0; j < max_r_c; j++)
            {
                var is_to = rand.Next(0, 10);

                normal_blocks.Add(new Block(con_x + gap, con_y + gap, 2, false, Color.Pink));
                changing_blocks.Add(new Block(con_x + gap, con_y + gap, 2, false, colors[rand.Next(0, colors.Length)]));
                con_x += size + gap;
            }
            con_x = initial_x;
            con_y += size + gap;

        }

        total_size = (max_r_c * size + (max_r_c * gap)) + gap;


        // creating the changing block which will transit throught the period the game.        
        var render1 = rand.Next(0, normal_blocks.Count - 1);
        var render2 = rand.Next(0, normal_blocks.Count);
        this.changing_blocks[render1].is_to_show = true;
        this.changing_blocks[(render2 == render1) ? render2++ : render2].is_to_show = true;

    }




}



// That's all
// Enjoy The game
// If you find any error in this part so let us know we will definitely fix it,or having any idea to improve it so do let us know.
// For contact  Email:-satyameshmalimern123@gmail.com


// Copyright©️ by AJTA's
