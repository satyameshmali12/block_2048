

namespace Game_2048;

public class Basicfunc
{
    // change Game1 class with your main class in below and the constructor
    Game1 basifuncgame;

    // only game screen
    public Basicfunc(Game1 game)
    {
        basifuncgame = game;
    }
    // with width and height
    public Basicfunc(Game1 game,int width,int height,bool fullscreen)
    {
        basifuncgame = game;
        setscreenorientation(width,height);
        setfullscreen(fullscreen);
    }

    // screen functions ....

    // set the screen orientation
    public void setscreenorientation(int width,int height)
    {
        basifuncgame._graphics.PreferredBackBufferWidth=width;
        basifuncgame._graphics.PreferredBackBufferHeight=height;
        basifuncgame._graphics.ApplyChanges();
    }

    // set the screen mode
    public void setfullscreen(bool fullscreen)
    {
        basifuncgame._graphics.IsFullScreen=fullscreen;
        basifuncgame._graphics.ApplyChanges();
    }

    // requires the gametime of the game
    public float getframerate(GameTime gameTime){
        float frameRate = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
        return frameRate;
    }

    public void settitle(string title){
        basifuncgame.Window.Title=title;
    }
    

    // file path as per the Content.mgcb.
    // using monogame extension to configure the Content.mgcb file.
    public Texture2D loadtexture(string filepath){
        return basifuncgame.Content.Load<Texture2D>(filepath);
    }

    // method for displaying the image or the loaded texture

    // without source reatangle
    public void displayimg(Texture2D texture,Vector2 position,float rotation,Color color,Vector2 origin,Vector2 scale,SpriteEffects effects=SpriteEffects.None,int depth=0,bool isblend=false)
    {
        beginblend(isblend);
        basifuncgame._spriteBatch.Draw(texture,position,null,color,rotation,origin,scale,effects,depth);
        basifuncgame._spriteBatch.End();
    }

    // with source rectangle
    public void displayimg(Texture2D texture,Vector2 position,float rotation,Color color,Rectangle sourcerectangle,Vector2 origin,Vector2 scale,SpriteEffects effects=SpriteEffects.None,int depth=0,bool isblend=false)
    {
        beginblend(isblend);
        basifuncgame._spriteBatch.Draw(texture,position,sourcerectangle,color,rotation,origin,scale,effects,depth);
        basifuncgame._spriteBatch.End();
    }

    // simple display with only texture position and color
    public void displayimg(Texture2D texture, Vector2 position, Color color,bool isblend=false){
        beginblend(isblend);
        basifuncgame._spriteBatch.Draw(texture,position,color);
        basifuncgame._spriteBatch.End();
    }

    // display image with the destination rectangle                 :- recommended
    public void displayimgrect(Texture2D texture,Rectangle destinationRectangle,bool isblend=false){
        beginblend(isblend);
        basifuncgame._spriteBatch.Draw(texture,destinationRectangle,Color.White);
        basifuncgame._spriteBatch.End();
    }

    public void displayimgrect(Texture2D texture, Rectangle destinationRectangle, Color color, float rotation=0, SpriteEffects effects=SpriteEffects.None,bool isblend=false)
    {
        beginblend(isblend);
        basifuncgame._spriteBatch.Draw(texture,destinationRectangle,null,color,0,Vector2.One,effects,rotation);
        basifuncgame._spriteBatch.End();
    }




    // to load the music
    public SoundEffect loadmusic(string filepath){
        return basifuncgame.Content.Load<SoundEffect>(filepath);
    }

    // load the music and get it's instance                         :- recommemded
    public SoundEffectInstance get_load_instance(string filepath,bool islooped=false){
        SoundEffect se = basifuncgame.Content.Load<SoundEffect>(filepath);
        SoundEffectInstance sei = se.CreateInstance();
        sei.IsLooped = islooped;
        return sei;
    }

    // text

    /// to load the text
    public SpriteFont loadfont(string fontname){
        SpriteFont font=basifuncgame.Content.Load<SpriteFont>(fontname);
        return font;
    }

    public void drawtext(SpriteFont font,string text,Vector2 position,Color color,Vector2 scale,bool isblend=false,float rotation=0){
        beginblend(isblend);
        basifuncgame._spriteBatch.DrawString(font, text,position,color,rotation,Vector2.One,scale,SpriteEffects.None,0);
        basifuncgame._spriteBatch.End();
    }

    public void drawtext(SpriteFont font,string text,Vector2 position,Color color,bool isblend=false,float rotation=0){
        beginblend(isblend);
        basifuncgame._spriteBatch.DrawString(font, text,position,color,rotation,Vector2.One,Vector2.One,SpriteEffects.None,0);
        basifuncgame._spriteBatch.End();
    }

    public void draw_load_text(string fontname,string text,Vector2 position,Color color,Vector2 scale,bool isblend=false,float rotation = 0){
        SpriteFont font =basifuncgame.Content.Load<SpriteFont>(fontname);
        beginblend(isblend);
        basifuncgame._spriteBatch.DrawString(font, text,position,color,rotation,Vector2.One,scale,SpriteEffects.None,0);
        basifuncgame._spriteBatch.End();
    }



    // function used in the class to enhance the reuseability...
    public void beginblend(bool isblend){
        if(isblend)basifuncgame._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);
        else basifuncgame._spriteBatch.Begin();
    }

    // disl :- short for draw_item_structure_layout
    public void disl(int x,int y,int width,int height,int dot_size,int dot_size2,string url_to_texture){
        displayimgrect(basifuncgame.Content.Load<Texture2D>(url_to_texture),new Rectangle(x,y,dot_size,dot_size));
        displayimgrect(basifuncgame.Content.Load<Texture2D>(url_to_texture),new Rectangle(x+width-dot_size,y,dot_size,dot_size));
        displayimgrect(basifuncgame.Content.Load<Texture2D>(url_to_texture),new Rectangle(x,y+height-dot_size,dot_size,dot_size));
        displayimgrect(basifuncgame.Content.Load<Texture2D>(url_to_texture),new Rectangle(x+width-dot_size,y+height-dot_size,dot_size,dot_size));
        displayimgrect(basifuncgame.Content.Load<Texture2D>(url_to_texture),new Rectangle(x+width/2-dot_size2/2,y+height/2-dot_size2/2,dot_size2,dot_size2));
    }

    
}