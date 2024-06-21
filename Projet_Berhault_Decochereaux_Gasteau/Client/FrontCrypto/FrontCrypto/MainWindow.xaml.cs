using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace PROJET_WPF_PROMEO_210624_CLIENT
{
    public partial class MainWindow : Window
    {
        // Constructeur de la fenêtre principale
        public MainWindow()
        {
            InitializeComponent();
            // Associe l'événement KeyDown de la zone de texte à la méthode Zone_Texte_Verif_ETR
            Zone_Texte_Utilisateur.KeyDown += Zone_Texte_Verif_ETR;
            // Sélectionne par défaut le bouton radio de chiffrement César
            Bouton_Caesar.IsChecked = true;
        }

        // Méthode appelée lorsque l'utilisateur appuie sur une touche dans la zone de texte
        private void Zone_Texte_Verif_ETR(object sender, KeyEventArgs e)
        {
            // Vérifie si la touche appuyée est "Entrée"
            if (e.Key == Key.Enter)
            {
                // Prépare le texte pour l'envoi (mise en majuscules et suppression des espaces)
                string Texte_Brut = Transformation_Pre_Envoi(Zone_Texte_Utilisateur.Text);

                // Vérifie que le texte ne contient que des lettres
                if (!IsTextValid(Texte_Brut))
                {
                    // Affiche un message d'erreur si le texte contient des chiffres
                    MessageBox.Show("Les chiffres ne sont pas acceptés");
                    return;
                }

                // Détermine le type de chiffrement sélectionné
                char type_chiffrement = Choix_Methode_Chiffrement();
                // Formate la requête à envoyer au serveur
                string request = $"{type_chiffrement}|{Texte_Brut}";
                // Envoie la requête au serveur et récupère la réponse
                string Texte_edited = Envoi_Vers_Serveur(request);
                // Affiche le texte chiffré dans l'interface utilisateur
                Resultat_Serveur.Text = Texte_edited;
            }
        }

        // Méthode pour transformer le texte avant l'envoi au serveur
        private string Transformation_Pre_Envoi(string input)
        {
            // Convertit le texte en majuscules et remplace les espaces par des chaînes vides
            return input.ToUpper().Replace(" ", "");
        }

        // Méthode pour vérifier que le texte ne contient que des lettres
        private bool IsTextValid(string text)
        {
            // Utilise une expression régulière pour vérifier que le texte ne contient que des lettres de a à z ou de A à Z
            return Regex.IsMatch(text, @"^[a-zA-Z]+$");
        }

        // Méthode pour déterminer la méthode de chiffrement sélectionnée
        private char Choix_Methode_Chiffrement()
        {
            // Retourne le caractère correspondant à la méthode de chiffrement sélectionnée
            if (Bouton_Caesar.IsChecked == true) return 'C';
            if (Bouton_Playfair.IsChecked == true) return 'P';
            if (Bouton_Substitution.IsChecked == true) return 'S';
            // Lance une exception si aucune méthode n'est sélectionnée (ce cas ne devrait pas se produire)
            throw new InvalidOperationException("Méthode de chiffrement inconnue");
        }

        // Méthode pour envoyer la requête au serveur et recevoir la réponse
        static string Envoi_Vers_Serveur(string texte)
        {
            // Convertit le texte en tableau de bytes
            byte[] b_texte = Encoding.ASCII.GetBytes(texte);

            // Crée un socket TCP pour la connexion au serveur
            Socket Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Connecte le socket au serveur sur l'adresse locale et le port 6666
            Sock.Connect("localhost", 6666);

            // Envoie les données au serveur
            Sock.Send(b_texte);

            // Prépare un tableau de bytes pour recevoir la réponse du serveur
            byte[] b_text_chiffred = new byte[256];

            // Lit la réponse du serveur
            Sock.Receive(b_text_chiffred);

            // Ferme proprement la connexion
            Sock.Shutdown(SocketShutdown.Both);
            Sock.Close();

            // Convertit la réponse en chaîne de caractères et la retourne
            string text_chiffred = Encoding.ASCII.GetString(b_text_chiffred);
            return text_chiffred;
        }

        // Méthode appelée lorsque le texte dans la zone de texte est modifié
        private void Zone_Texte_Utilisateur_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Réinitialise le texte du résultat du serveur
            Resultat_Serveur.Text = string.Empty;
        }
    }
}
