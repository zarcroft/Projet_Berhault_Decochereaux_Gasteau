using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class ChiffrementServeur
{
    private TcpListener _listener;

    // Constructeur pour initialiser le serveur à un port donné
    public ChiffrementServeur(int port)
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start(); // Démarrage du serveur
        Console.WriteLine("Serveur démarré, en attente de clients...");
    }

    // Méthode pour exécuter le serveur en boucle
    public void Run()
    {
        while (true)
        {
            Console.WriteLine("Attente client");
            TcpClient client = _listener.AcceptTcpClient(); // Accepte une connexion client
            Console.WriteLine("Client connecté");

            HandleClient(client); // Gérer la connexion client

            client.Close(); // Fermer la connexion avec le client
        }
    }

    // Méthode pour gérer la connexion avec un client
    private void HandleClient(TcpClient client)
    {
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length); // Lire les données du client

            string request = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim(); // Convertir les octets en chaîne de caractères
            Console.WriteLine($"{DateTime.Now} - {client.Client.RemoteEndPoint} - {request}");

            string response;
            try
            {
                response = Chiffrer(request); // Chiffrer la requête
            }
            catch (Exception ex)
            {
                response = ex.Message; // En cas d'erreur, renvoyer le message d'erreur
            }

            byte[] responseBytes = Encoding.ASCII.GetBytes(response); // Convertir la réponse en octets
            stream.Write(responseBytes, 0, responseBytes.Length); // Envoyer la réponse au client
        }
    }

    // Méthode pour chiffrer la requête
    private string Chiffrer(string request)
    {
        string[] parts = request.Split('|'); // Séparer la méthode de chiffrement et le texte

        if (parts.Length < 2)
        {
            throw new ArgumentException("Format de requête invalide"); // Vérifier le format de la requête
        }

        char method = parts[0][0]; // Méthode de chiffrement (C, P, S)
        string textToEncrypt = parts[1]; // Texte à chiffrer

        string encryptedText;
        switch (method)
        {
            case 'C':
                encryptedText = ChiffrementCesar(textToEncrypt); // Chiffrement de César
                break;
            case 'P':
                encryptedText = ChiffrementPlayfair(textToEncrypt); // Chiffrement de Playfair
                break;
            case 'S':
                encryptedText = ChiffrementSubstitution(textToEncrypt); // Chiffrement par substitution
                break;
            default:
                throw new ArgumentException("Méthode de chiffrement inconnue");
        }

        return encryptedText;
    }

    // Méthode pour le chiffrement de César
    private string ChiffrementCesar(string text)
    {
        int shift = 3; // Décalage de 3 positions dans l'alphabet
        char[] chars = text.ToCharArray();

        for (int i = 0; i < chars.Length; i++)
        {
            if (chars[i] >= 'A' && chars[i] <= 'Z') // Lettre majuscule
            {
                chars[i] = (char)(chars[i] + shift);
                if (chars[i] > 'Z')
                {
                    chars[i] = (char)(chars[i] - 26); // Retour au début de l'alphabet si nécessaire
                }
            }
            else if (chars[i] >= 'a' && chars[i] <= 'z') // Lettre minuscule
            {
                chars[i] = (char)(chars[i] + shift);
                if (chars[i] > 'z')
                {
                    chars[i] = (char)(chars[i] - 26); // Retour au début de l'alphabet si nécessaire
                }
            }
        }

        return new string(chars);
    }

    // Méthode pour le chiffrement de Playfair
    private string ChiffrementPlayfair(string text)
    {
        // Définition de la clé Playfair
        char[,] key = {
            {'B', 'Y', 'D', 'G', 'Z'},
            {'J', 'S', 'F', 'U', 'P'},
            {'L', 'A', 'R', 'K', 'X'},
            {'C', 'O', 'I', 'V', 'E'},
            {'Q', 'N', 'M', 'H', 'T'}
        };

        // Remplacement des W par des X
        text = text.Replace('W', 'X');

        // Complétion du message pour obtenir une longueur paire
        if (text.Length % 2 != 0)
        {
            text += 'X';
        }

        // Découpage du texte en blocs de deux lettres
        string encryptedText = "";
        for (int i = 0; i < text.Length; i += 2)
        {
            char c1 = text[i];
            char c2 = text[i + 1];

            // Recherche des positions des lettres dans la clé
            int x1 = -1, y1 = -1, x2 = -1, y2 = -1;
            for (int j = 0; j < 5; j++)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (key[j, k] == c1)
                    {
                        x1 = j;
                        y1 = k;
                    }
                    if (key[j, k] == c2)
                    {
                        x2 = j;
                        y2 = k;
                    }
                }
            }

            // Vérifier que les positions sont valides
            if (x1 == -1 || y1 == -1 || x2 == -1 || y2 == -1)
            {
                throw new ArgumentException("Caractère non trouvé dans la clé Playfair");
            }

            // Chiffrement du bloc
            if (x1 == x2) // Même ligne
            {
                encryptedText += key[x1, (y1 + 1) % 5];
                encryptedText += key[x2, (y2 + 1) % 5];
            }
            else if (y1 == y2) // Même colonne
            {
                encryptedText += key[(x1 + 1) % 5, y1];
                encryptedText += key[(x2 + 1) % 5, y2];
            }
            else // Rectangle
            {
                encryptedText += key[x1, y2];
                encryptedText += key[x2, y1];
            }
        }

        return encryptedText;
    }

    // Méthode pour le chiffrement par substitution
    private string ChiffrementSubstitution(string text)
    {
        // Définition de la clé de substitution
        string cleaire = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
        string clechiffree = "HIJKLMNVWXYZBCADEFGOPQRSTU";

        // Remplacement des lettres du texte clair par les lettres chiffrées
        string encryptedText = "";
        foreach (char c in text.ToUpper())
        {
            int index = cleaire.IndexOf(c);
            if (index != -1)
            {
                encryptedText += clechiffree[index];
            }
            else
            {
                encryptedText += c;
            }
        }

        return encryptedText;
    }
}

class Program
{
    static void Main(string[] args)
    {
        ChiffrementServeur serveur = new ChiffrementServeur(6666);
        serveur.Run(); // Lancer le serveur
    }
}
