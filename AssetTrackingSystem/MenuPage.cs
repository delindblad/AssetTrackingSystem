namespace MenuKit;

//Abstract base class for menu pages
public abstract class MenuPage
{
    public MenuPage? Context { get; set; } = null;
    public MenuPage? Parent { get; set; }
    public List<MenuPage> ChildPages { get; set; }
    public string Title { get; set; }

    public MenuPage(string title, MenuPage? parent)
    {
        Title = title;
        ChildPages = new List<MenuPage>();
        Context = Parent = parent;
    
        
    }
    //Adds child pages
    public void AddChildPage(MenuPage page)
    {
        ChildPages.Add(page);
    }

    //Displays information when loading page
    public abstract void Display();
    //Handle interaction here, returns int signaling result
    public abstract int Interact();
    //Runs page, returns new context
    public abstract MenuPage Run();
}