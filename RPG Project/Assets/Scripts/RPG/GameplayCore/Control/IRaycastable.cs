namespace RPG.GameplayCore.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController interactController);

        CursorType GetCursorType();
    }
}