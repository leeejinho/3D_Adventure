public interface IInteractable
{
    public string GetInteractPrompt();
    public void OnInteract();
}

public interface IThrowable
{
    public void ThrowItem();
}
