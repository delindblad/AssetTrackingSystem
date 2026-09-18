namespace AssetTrackingSystem;

public abstract class MenuPage
{
    public MenuPage? Result { get; set; } = null;
    public MenuPage? Parent { get; set; }
    public List<MenuPage> ChildPages { get; set; }
    public string Title { get; set; }

    public MenuPage(string title, MenuPage? parent)
    {
        Title = title;
        ChildPages = new List<MenuPage>();
        Result = Parent = parent;
    
        
    }
    public void AddChildPage(MenuPage page)
    {
        ChildPages.Add(page);
    }

    public abstract void Display();
    public abstract void Interact();
    
    public abstract MenuPage Run();
}