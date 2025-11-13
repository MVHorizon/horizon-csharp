namespace HorizonChat.Services
{
    public class UserStateService
    {
        public string Username { get; private set; } = string.Empty;
        public bool IsUserAuthenticated => !string.IsNullOrWhiteSpace(Username);

        public event Action? OnChange;

        public void SetUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(username));
            }

            Username = username;
            NotifyStateChanged();
        }

        public void ClearUsername()
        {
            Username = string.Empty;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
