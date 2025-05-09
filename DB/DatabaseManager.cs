using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using photocool.Models;

namespace photocool.DB;

public class DatabaseManager
{
    public const int DUPLICATE_ENTRY = 1062;
    
    private static readonly string _connectionString = "Server=localhost;Port=3306;Database=photocool;Uid=root;Pwd=root;";
    
    /// <summary>
    /// Fonction qui renvoie tout les tags dans un reader 
    /// </summary>
    /// <returns>MySqlDataReader</returns>
    public static List<string> getAllTags()
    {
        List<string> tags = new List<string>();
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM `Tag`";
            MySqlCommand command = new MySqlCommand(query, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tags.Add(reader.GetString("tag"));
                }
            }
        }
        return tags;
    }

    public static Dictionary<long, long> getTagFamily()
    {
        Dictionary<long, long> tagFamily = new();
        string query = "SELECT * FROM `TagFamille`";
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tagFamily.Add(reader.GetInt64("tag_fils"), reader.GetInt64("tag_parent"));
                }
            }
        }

        return tagFamily;
    }

    public static Dictionary<long, string> getAllTagsAndIds()
    {
        Dictionary<long, string> tagsAndIds = new();
        string query = "SELECT * FROM `Tag`";
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tagsAndIds.Add(reader.GetInt64("id"), reader.GetString("tag"));
                }
            }
        }

        return tagsAndIds;
    }

    public static byte[] getImage(long id)
    {
        byte[] data = null;
        string query = "SELECT image FROM `Images` WHERE id = @id";
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    data = (byte[])reader["image"];
                }
            }
        }

        return data;
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

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@tag", tag);
            command.ExecuteNonQuery();
            return command.LastInsertedId;
        }
    }

    /// <summary>
    /// Fonction qui renomme un tag de la BDD
    /// </summary>
    /// <param name="tagToModify">Le nom du tag à modifier</param>
    /// <param name="newTag">Le nouveau nom du tag</param>
    public static void modifyTag(string tagToModify, string newTag)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE `Tag` SET `tag`=@newTag WHERE `tag`=@tagToModify";

            MySqlCommand command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@newTag", newTag);
            command.Parameters.AddWithValue("@tagToModify", tagToModify);
            command.ExecuteNonQuery();
        }
    }
    
    /// <summary>
    /// Fonction qui modifie le parent d'un tag de la BDD
    /// </summary>
    /// <param name="tagToModify">Le nom du tag à modifier</param>
    /// <param name="newTagParent">Le nouveau parent du tag</param>
    public static void modifyTagParent(string tagToModify, string newTagParent)
    {
        long id = getTagId(tagToModify);
        long idparent = getTagId(newTagParent);
        if (id == -1L || idparent == -1L)
        {
            throw new Exception("Erreur l'un des deux tag n'existe pas");
        }
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "UPDATE `TagFamille` SET `tag_parent`=@idparent WHERE `tag_fils`=@idtag";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idtag", id);
                command.Parameters.AddWithValue("@idparent", idparent);
                command.ExecuteNonQuery();
            }
        }
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
            string query = "INSERT INTO `TagFamille` (`tag_fils`, `tag_parent`) VALUES (@idtag, @idparent);";
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
    /// <exception cref="Exception">si le tag n'existe pas dans la bdd</exception>
    public static void removeParentFromTag(string tag)
    {
        long id = getTagId(tag);
        if (id == -1L)
        {
            throw new Exception("Erreur le tag n'existe pas");
        }
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM `TagFamille` WHERE `tag_fils`=@id;";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
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

    /// <summary>
    /// renvoie les enfants directs d'un tag
    /// </summary>
    /// <param name="tag">tag donné</param>
    /// <param name="childs"> dictionnaire des enfants, peut être null</param>
    /// <returns>le dictionnaire des enfants du tags (paire ID,Tag)</returns>
    public static Dictionary<long,string> getChildsOfTag(string tag,Dictionary<long,string> childs=null)
    {
        if (childs is null)
        {
            
            childs = new Dictionary<long, string>();
        }

        long id = getTagId(tag);
        if (id != -1L)
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM `TagFamille` WHERE `tag_parent`=@tag";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@tag", id);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long idOfChild = reader.GetInt32("tag_fils");
                            string tagOfChild = getTag(idOfChild);
                            
                            childs.Add(idOfChild,tagOfChild);
                        }
                    }
                }
            }
        return childs;
    }

    /// <summary>
    /// renvoie le tag à partir d'un id bdd,renvoi un string vide s'il n'existe pas
    /// </summary>
    /// <param name="id">id du tag</param>
    /// <returns>tag du tag</returns>
    public static string getTag(long id)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM `Tag` WHERE `id`=@id";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return reader.GetString("tag");
                    }
                }
                
            }
        }
        return "";
    }

    /// <summary>
    /// ajoute une image en bdd
    /// </summary>
    /// <param name="imagePath">chemin du fichier de l'image</param>
    /// <param name="nom">nom de l'image</param>
    /// <returns>l'id de l'image insérée</returns>
    public static long addImage(string imagePath)
    {
        byte[] imageData = File.ReadAllBytes(imagePath);
        byte[] miniature = ThumbnailPhotocool.CreateThumbnailFromData(imageData);
        long id = -1L;
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO Images (image, miniature) values (@image, @miniature);";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@image", imageData);
                command.Parameters.AddWithValue("@miniature", miniature);
                command.ExecuteNonQuery();
                id = command.LastInsertedId;
            }
            
        }
        return id;
    }

    /// <summary>
    /// ajoute un tag à une image
    /// </summary>
    /// <param name="nom">nom de l'image</param>
    /// <param name="tag">tag a ajouter</param>
    /// <param name="imageId">id de l'image (optionnel mais accelère)</param>
    /// <param name="tagId">id du tag (optionnel mais accelère)</param>
    /// <returns> l'id de la relation créée</returns>
    public static long addTagToImage(long imageId, string tag, long tagId=-1L)
    {
        long tagImageId = -1L;
        if (tagId == -1L)
        {
            
            tagId = getTagId(tag);
            Console.WriteLine(tagId);
        }

        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "INSERT INTO `TagImages` (`image_id`, `tag_id`) VALUES (@imageId, @tagId);";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@imageId", imageId);
                
                command.Parameters.AddWithValue("@tagId", tagId);
                command.ExecuteNonQuery();
                tagImageId = command.LastInsertedId;
            }
        }
        return tagImageId;
    }

    /// <summary>
    /// Donne l'id d'une image avec son nom
    /// </summary>
    /// <param name="nom">nom de l'image</param>
    /// <returns>l'id de l'image et -1 si elle n'est pas trouvée</returns>
    public static long getImageId(string nom)
    {
        long imageId = -1L;
        string query = "SELECT * FROM `Images` WHERE `nom`=@nom";
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@nom", nom);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        imageId = reader.GetInt32("id");
                    }
                }
            }
        }
        return imageId;
    }
    
    /// <summary>
    /// enlève une image de la bdd
    /// </summary>
    /// <param name="nom">id de l'image</param>
    public static void removeImage(long imageId)
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "DELETE FROM `Images` WHERE `Images`.`Id`=@imageId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@imageId",imageId);
                command.ExecuteNonQuery();
            }
        }
    }
    
    /// <summary>
    /// récupère les tags d'une image
    /// </summary>
    /// <param name="nom">id de l'image</param>
    public static List<string> GetImageTags(long imageId)
    {
        List<string> tags = new List<string>();
        string query = @"SELECT tag FROM `Tag` WHERE `Tag`.`id` IN
                            (
                                SELECT tag_id FROM `TagImages` WHERE image_id=@imageId
                            )";
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@imageId",imageId);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tags.Add(reader.GetString("tag"));
                    }
                }
            }
        }

        return tags;
    }
    
    /// <summary>
    /// retire les tags d'une image
    /// </summary>
    /// <param name="nom">id de l'image</param>
    public static void RemoveImageTags(long imageId)
    {
        string query = @"DELETE FROM `TagImages` WHERE `image_id`=@imageId";
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@imageId",imageId);
                command.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Enleve un tag d'une image
    /// </summary>
    /// <param name="nom">nom de l'image</param>
    /// <param name="tag">tag de l'image</param>
    /// <param name="imageId">id de l'immage (optionnel)</param>
    /// <param name="tagId">id du tag (optionnel)</param>
    public static void removeTagFromImage(string nom, string tag, long imageId = -1L, long tagId = -1L)
    {
        if(imageId == -1L)
            imageId = getImageId(nom);
        if(tagId == -1L)
            tagId = getTagId(tag);
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "delete from `TagImages` WHERE `image_id`=@imageId AND `tag_id`=@tagId";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@imageId", imageId);
                command.Parameters.AddWithValue("@tagId", tagId);
                command.ExecuteNonQuery();
            }
        }
    }

    public static IEnumerable<ThumbnailPhotocool> getAllImagesAsStream()
    {
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query = "SELECT id, miniature FROM `Images`";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] thumbnail = (byte[])reader["miniature"];
                        yield return new ThumbnailPhotocool(reader.GetInt64("id"), thumbnail);
                    }
                }
            }   
        }
    }

    public static IEnumerable<ThumbnailPhotocool> getImagesMustSatisfyAnyFilterAsStream(List<string> filters)
    {
        List<long> ids = new List<long>();
        foreach (string tag in filters)
        {
            long id = getTagId(tag);
            if (id != -1)
                ids.Add(getTagId(tag));
        }
        
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            
            // get all tag relations
            string query = "SELECT * FROM `TagFamille`";
            Dictionary<long, long> relations = new();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        relations.Add(reader.GetInt64("tag_fils"), reader.GetInt64("tag_parent"));
                    }
                }
            }
            
            // create tag hierarchy
            Dictionary<long, HashSet<long>> hierarchy = new();
            foreach (KeyValuePair<long, long> relation in relations) // key is child, value is parent
            {
                if (!hierarchy.ContainsKey(relation.Value))
                    hierarchy.Add(relation.Value, new HashSet<long>());
                hierarchy[relation.Value].Add(relation.Key);
            }
            
            // get relevant ids
            HashSet<long> relevantIds = new();
            foreach (long id in ids)
            {
                relevantIds.Add(id);
                AddDescendants(id, hierarchy, relevantIds);
            }

            query = $@"SELECT id, miniature
                        FROM `Images`
                        WHERE id IN (
                            SELECT DISTINCT image_id
                            FROM `TagImages`
                            WHERE tag_id IN ({ string.Join(", ", relevantIds)})
                        )";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] thumbnail = (byte[])reader["miniature"];
                        yield return new ThumbnailPhotocool(reader.GetInt64("id"), thumbnail);
                    }
                }
            }
        }
    }
    
    public static IEnumerable<ThumbnailPhotocool> getImagesMustSatisfyAllFiltersAsStream(List<string> filters)
    {
        List<long> ids = new List<long>();
        foreach (string tag in filters)
        {
            long id = getTagId(tag);
            if (id != -1)
                ids.Add(getTagId(tag));
        }
        
        using (MySqlConnection connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string query;

            // get all tag relations
            query = "SELECT * FROM `TagFamille`";
            Dictionary<long, long> relations = new();
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        relations.Add(reader.GetInt64("tag_fils"), reader.GetInt64("tag_parent"));
                    }
                }
            }
            
            // get all image tags
            Dictionary<long, HashSet<long>> imageTags = new();
            query = "SELECT image_id, tag_id FROM `TagImages`";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long imageId = reader.GetInt64("image_id");
                        long tagId = reader.GetInt64("tag_id");
                        
                        if (!imageTags.ContainsKey(imageId))
                            imageTags.Add(imageId, new HashSet<long>());
                        
                        imageTags[imageId].Add(tagId);
                    }
                }
            }
            
            // create tag hierarchy
            Dictionary<long, HashSet<long>> hierarchy = new();
            foreach (KeyValuePair<long, long> relation in relations) // key is child, value is parent
            {
                if (!hierarchy.ContainsKey(relation.Value))
                    hierarchy.Add(relation.Value, new HashSet<long>());
                hierarchy[relation.Value].Add(relation.Key);
            }
            
            // get relevant ids
            HashSet<long> relevantIds = new(ids);

            // go through all images and apply filters
            query = "SELECT id, miniature FROM `Images`";
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long imageId = reader.GetInt64("id");
                        HashSet<long> tags;
                        if (imageTags.ContainsKey(imageId))
                            tags = imageTags[imageId];
                        else
                        {
                            continue;
                        }

                        // check if all tags are satisfied
                        Dictionary<long, bool> satisfiedIds = new();
                        foreach (long id in relevantIds)
                        {
                            satisfiedIds.Add(id, false);
                        }

                        bool allSatisfied = true;
                        foreach (long id in tags)
                        {
                            bool found = false;
                            foreach (long relevantId in relevantIds)
                            {
                                if (IsDescendantOrEqualOf(id, relevantId, relations))
                                {
                                    satisfiedIds[relevantId] = true;
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                            {
                                allSatisfied = false;
                                break;
                            }
                        }

                        if (allSatisfied)
                        {
                            foreach (KeyValuePair<long, bool> id in satisfiedIds)
                            {
                                if (id.Value == false)
                                {
                                    allSatisfied = false;
                                    break;
                                }
                            }
                        }
                        
                        if (allSatisfied)
                        {
                            byte[] thumbnail = (byte[])reader["miniature"];
                            yield return new ThumbnailPhotocool(reader.GetInt64("id"), thumbnail);
                        }
                    }
                }
            }
        }
    }

    private static void AddDescendants(long parentId, Dictionary<long, HashSet<long>> hierarchy, HashSet<long> relevantIds)
    {
        if (hierarchy.ContainsKey(parentId))
        {
            HashSet<long> childrenIds = hierarchy[parentId];
            foreach (long childId in childrenIds)
            {
                relevantIds.Add(childId);
                AddDescendants(childId, hierarchy, relevantIds);
            }
        }
    }

    private static bool IsDescendantOrEqualOf(long descendant, long ancestor, Dictionary<long, long> relations)
    {
        if (ancestor == descendant)
            return true;
        
        while (relations.ContainsKey(descendant))
        {
            long current = relations[descendant];
            if (current == ancestor)
                return true;
            
            descendant = current;
        }

        return false;
    }
}
