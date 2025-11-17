namespace HorizonChat.Services
{
    public class UserStateService
    {
        private static readonly string[] PredefinedStatuses = new[]
        {
            "Available",
            "Busy",
            "Away",
            "Do Not Disturb"
        };

        public string Username { get; private set; } = string.Empty;
        public string Status { get; private set; } = "Available";
        public bool IsUserAuthenticated => !string.IsNullOrWhiteSpace(Username);

        public IEnumerable<string> AvailableStatuses => PredefinedStatuses;

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

        public void SetStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException("Status cannot be empty.", nameof(status));
            }

            if (status.Length > 30)
            {
                throw new ArgumentException("Status cannot exceed 30 characters.", nameof(status));
            }

            Status = status;
            NotifyStateChanged();
        }

        public void ClearUsername()
        {
            Username = string.Empty;
            Status = "Available";
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
