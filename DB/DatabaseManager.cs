using System;
using Avalonia.Win32.Interop.Automation;
using MySql.Data.MySqlClient;
using Tmds.DBus.Protocol;

namespace photocool.DB;

public class DatabaseManager
{
    private static readonly string _connectionString = "Server=localhost;Port=3306;Database=photocool;Uid=root;Pwd=root;";
/// <summary>
/// Fonction qui renvoie tout les tags dans un reader 
/// </summary>
/// <returns>MySqlDataReader</returns>
    public static MySqlDataReader getAllTags()
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM `Tag`";
            MySqlCommand command = new MySqlCommand(query, connection);
            return command.ExecuteReader();

            
        }
    }
/// <summary>
/// Fonction qui ajoute un tag et renvoi son id.
/// </summary>
/// <param name="tag">tag à ajouter</param>
/// <returns>id du tag ajouté, -1 en cas d'erreur.</returns>

    public static long addTag(string tag)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO `Tag` (`Tag`) VALUES (@tag)";
            try
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@tag", tag);
                command.ExecuteNonQuery();
                return command.LastInsertedId;
            }
            catch (MySqlException e)
            {
                if (e.Number == 1062) // 1062 = Duplicate entry
                {
                    Console.WriteLine("Doublon détecté !");
                }
                else
                {
                    Console.WriteLine("Erreur MySQL : " + e.Message);
                }
            }
            
        }

        return -1L;
    }
/// <summary>
/// Fonction qui supprime un tag avec son tag de la bdd
/// </summary>
/// <param name="tag">String</param>
    public static void removeTag(string tag)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM `Tag` WHERE `Tag`=@tag";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tag", tag);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("Erreur MySQL : " + e.Message);
                }
            }
        }
    }
/// <summary>
///  Ajoute un tag à la bdd et l'associe à un parent.
/// </summary>
/// <param name="tag">Tag à ajouter</param>
/// <param name="parent">Tag du parent.</param>
/// <returns>ID du tag ajouté</returns>
    public static long addTagWithParent(string tag, string parent)
    {
        long id = addTag(tag);
        addParentToTag(tag, parent);
        return id;
    }
/// <summary>
/// ajoute un parent au tag
/// </summary>
/// <param name="tag">tag fils</param>
/// <param name="parent">tag parent</param>
/// <returns>l'id de la ligne de la relation en bdd</returns>
/// <exception cref="Exception">si l'un des deux tags n'existe pas</exception>
    public static long addParentToTag(string tag, string parent)
    {
        long id = getTagId(tag);
        long parentId = getTagId(parent);
        if (id == -1L || parentId == -1L)
        {
            throw new Exception("Erreur l'un des deux tag n'existe pas");
        }
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO `tag_famille` (`tag_fils`, `tag_parent`) VALUES (@idtag, @idparent);";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idtag", id);
                command.Parameters.AddWithValue("@idparent", parentId);
                try
                {
                    command.ExecuteNonQuery();
                    return command.LastInsertedId;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine("Erreur lors de l'ajout de la liaison parent-fils tag : " + e.Message);
                }
            }
        }
        return -1L;
    }
/// <summary>
/// Méthode qui supprime la relation parent enfant dans la bdd
/// </summary>
/// <param name="tag">tag fils</param>
/// <param name="parent">tag parent</param>
/// <exception cref="Exception">si l'un des deux tags n'existe pas dans la bdd</exception>
    public static void removeParentFromTag(string tag, string parent)
    {
        long id = getTagId(tag);
        long parentId = getTagId(parent);
        if (id == -1L || parentId == -1L)
        {
            throw new Exception("Erreur l'un des deux tag n'existe pas");
        }
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM `tag_famille` WHERE `tag`=@tag AND `tag_parent`=@idparent;";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idparent", parentId);
                command.Parameters.AddWithValue("@tag", tag);
                command.ExecuteNonQuery();
            }
            
        }

    }
/// <summary>
/// renvoie l'id du tag passé en paramètre
/// </summary>
/// <param name="tag">le tag de l'id cherché</param>
/// <returns>l'id du tag cherché -1 si il n'existe pas.</returns>
    public static long getTagId(string tag)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM `Tag` WHERE `Tag`=@tag";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tag", tag);
                using (var reader = command.ExecuteReader())
                {
                    return reader.Read() ? reader.GetInt32("id") : -1L;
                }
            }
        }
    }
}
