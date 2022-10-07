
namespace Game_2048;

public class Block
{
    public int x,y;
    public int value;

    // i_c_p    :- short for is_conjugation performed
    public bool is_conjugation_performed,is_to_show;
    public bool is_transitioning;
    public Color box_color;
    public Block(int x,int y,int initial_value,bool is_to_show,Color box_color){
        this.x = x;
        this.y = y;
        this.value = initial_value;
        this.is_conjugation_performed = false;
        this.is_to_show = is_to_show;
        this.is_transitioning = false;
        this.box_color = box_color;
        
    }
    
}