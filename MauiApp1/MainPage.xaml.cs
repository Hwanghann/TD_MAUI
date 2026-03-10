namespace MauiApp1;

public partial class MainPage : ContentPage
{
    string joueurActuel = "X";

    public MainPage()
    {
        InitializeComponent();
        ResetGame(null, null);
    }

    private void CaseCliquee(object sender, EventArgs e)
    {
        Button bouton = (Button)sender;

        if (bouton.Text != "")
            return;

        bouton.Text = joueurActuel;

        if (VerifierVictoire())
        {
            StatusLabel.Text = "Le joueur " + joueurActuel + " gagne !";
            DesactiverPlateau();
            return;
        }

        if (PlateauPlein())
        {
            StatusLabel.Text = "Match nul !";
            DesactiverPlateau();
            return;
        }

        joueurActuel = joueurActuel == "X" ? "O" : "X";
        StatusLabel.Text = "Tour du joueur " + joueurActuel;
    }

    bool VerifierVictoire()
    {
        var boutons = BoardGrid.Children.OfType<Button>().ToArray();

        string[,] board = new string[3, 3];

        int index = 0;

        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                board[r, c] = boutons[index].Text;
                index++;
            }
        }

        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                return true;

            if (board[0, i] != "" && board[0, i] == board[1, i] && board[1, i] == board[2, i])
                return true;
        }

        if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
            return true;

        if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
            return true;

        return false;
    }

    bool PlateauPlein()
    {
        foreach (var bouton in BoardGrid.Children.OfType<Button>())
        {
            if (bouton.Text == "")
                return false;
        }

        return true;
    }

    void DesactiverPlateau()
    {
        foreach (var bouton in BoardGrid.Children.OfType<Button>())
        {
            bouton.IsEnabled = false;
        }
    }

    void ResetGame(object sender, EventArgs e)
    {
        joueurActuel = "X";
        StatusLabel.Text = "Tour du joueur X";

        foreach (var bouton in BoardGrid.Children.OfType<Button>())
        {
            bouton.Text = "";
            bouton.IsEnabled = true;
        }
    }
}