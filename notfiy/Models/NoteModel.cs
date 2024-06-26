﻿using System;
using System.Collections.Generic;
using Npgsql;
using notfiy.Entities;
using notfiy.Core;
using notfiy.Helpers;
using System.Diagnostics;

namespace notfiy.Models
{
    internal class NoteModel : Model
    {
        public List<Note> GetAllNote(int idUser, int idStatus)
        {
            List<Note> ListNotes = new List<Note>();

            try
            {
                Connection.Open();
                string query = "SELECT * FROM notes WHERE id_user = @idUser AND id_status = @idStatus ORDER BY time_created DESC";

                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, Connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@idUser", idUser);
                    npgsqlCommand.Parameters.AddWithValue("@idStatus", idStatus);

                    using (NpgsqlDataReader reader = npgsqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Note note = new Note
                            {
                                IdNote = (int)reader["id_note"],
                                NoteName = (string)reader["note_name"],
                                Content = (string)reader["content"],
                                ImageUrl = reader["image_url"] != DBNull.Value ? (string)reader["image_url"] : null,
                                TimeCreated = (DateTime)reader["time_created"],
                                Pinned = (bool)reader["pinned"],
                                IdUser = (int)reader["id_user"],
                                IdLabel = reader["id_label"] != DBNull.Value ? (int?)reader["id_label"] : null,
                                IdStatus = (int)reader["id_status"]
                            };

                            ListNotes.Add(note);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Retrieval failed! Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return ListNotes;
        }

        public List<Note> GetAllNoteFromLabel(int idUser, int idLabel)
        {
            List<Note> ListNotes = new List<Note>();

            try
            {
                Connection.Open();
                string query = "SELECT * FROM notes WHERE id_user = @idUser AND id_label = @idLabel ORDER BY time_created DESC";

                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, Connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@idUser", idUser);
                    npgsqlCommand.Parameters.AddWithValue("@idLabel", idLabel);

                    using (NpgsqlDataReader reader = npgsqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Note note = new Note
                            {
                                IdNote = (int)reader["id_note"],
                                NoteName = (string)reader["note_name"],
                                Content = (string)reader["content"],
                                ImageUrl = reader["image_url"] != DBNull.Value ? (string)reader["image_url"] : null,
                                TimeCreated = (DateTime)reader["time_created"],
                                Pinned = (bool)reader["pinned"],
                                IdUser = (int)reader["id_user"],
                                IdLabel = reader["id_label"] != DBNull.Value ? (int?)reader["id_label"] : null,
                                IdStatus = (int)reader["id_status"]
                            };

                            ListNotes.Add(note);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Retrieval failed! Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return ListNotes;
        }


        public List<Note> SearchNotes(int idUser, int idStatus, string queryNoteName)
        {
            List<Note> ListNotes = new List<Note>();

            try
            {
                Connection.Open();
                string query = "SELECT * FROM notes WHERE id_user = @idUser AND id_status = @idStatus AND note_name ILIKE @NoteName ORDER BY time_created DESC";
                using (NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, Connection))
                {
                    npgsqlCommand.Parameters.AddWithValue("@idUser", idUser);
                    npgsqlCommand.Parameters.AddWithValue("@idStatus", idStatus);
                    npgsqlCommand.Parameters.AddWithValue("@NoteName", $"%{queryNoteName}%");

                    using (NpgsqlDataReader reader = npgsqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Note note = new Note
                            {
                                IdNote = (int)reader["id_note"],
                                NoteName = (string)reader["note_name"],
                                Content = (string)reader["content"],
                                ImageUrl = reader["image_url"] != DBNull.Value ? (string)reader["image_url"] : null,
                                TimeCreated = (DateTime)reader["time_created"],
                                Pinned = (bool)reader["pinned"],
                                IdUser = (int)reader["id_user"],
                                IdLabel = reader["id_label"] != DBNull.Value ? (int?)reader["id_label"] : null,
                                IdStatus = (int)reader["id_status"]
                            };

                            ListNotes.Add(note);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Retrieval failed! Error: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return ListNotes;
        }

        public Note? GetNoteById(int idNote)
        {
            try
            {
                Connection.Open();
                string query = "SELECT * FROM notes WHERE id_note = @idNote";
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, Connection))
                {
                    cmd.Parameters.AddWithValue("@idNote", idNote);

                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Note
                            {
                                IdNote = (int)reader["id_note"],
                                NoteName = (string)reader["note_name"],
                                Content = (string)reader["content"],
                                ImageUrl = reader["image_url"] != DBNull.Value ? (string)reader["image_url"] : null,
                                TimeCreated = (DateTime)reader["time_created"],
                                Pinned = (bool)reader["pinned"],
                                IdUser = (int)reader["id_user"],
                                IdLabel = reader["id_label"] != DBNull.Value ? (int?)reader["id_label"] : null,
                                IdStatus = (int)reader["id_status"]
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Retrieval failed! Error: " + ex.Message);
                return null;
            }
            finally
            {
                Connection.Close();
            }
        }


        public int CreateNote(Note note)
        {
            try
            {
                Connection.Open();
                string insert;
                if (note.IdLabel == null)
                {
                    insert = @"insert into notes (note_name, content, image_url, time_created, pinned, id_user, id_status)
                    values (@note_name, @content, @image_url, @time_created, @pinned, @id_user, @id_status) RETURNING id_note";
                }
                    else
                {
                    insert = @"INSERT INTO notes (note_name, content, image_url, time_created, pinned, id_user, id_label, id_status) 
                       VALUES (@note_name, @content, @image_url, @time_created, @pinned, @id_user, @id_label, @id_status)
                       RETURNING id_note";
                }
            using (NpgsqlCommand cmd = new NpgsqlCommand(insert, Connection))
                {
                    cmd.Parameters.AddWithValue("@note_name", note.NoteName);
                    cmd.Parameters.AddWithValue("@content", note.Content);
                    cmd.Parameters.AddWithValue("@image_url", note.ImageUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@time_created", note.TimeCreated);
                    cmd.Parameters.AddWithValue("@pinned", note.Pinned);
                    cmd.Parameters.AddWithValue("@id_user", note.IdUser);
                    if (note.IdLabel != null)
                    {
                        cmd.Parameters.AddWithValue("@id_label", note.IdLabel);
                    }
                    cmd.Parameters.AddWithValue("@id_status", note.IdStatus);
                    object? result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 0;
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Insert failed! Error: " + ex.Message);
                return 0;
            }
            finally
            {
                Connection.Close();
            }
        }


        public bool UpdateNote(Note note)
        {
            try
            {
                Connection.Open();
                string update;
                if (note.IdLabel == null)
                {
                    update = @"UPDATE notes SET 
                                note_name = @note_name, 
                                content = @content, 
                                image_url = @image_url, 
                                time_created = @time_created, 
                                pinned = @pinned,
                                id_user = @id_user, 
                                id_status = @id_status 
                               WHERE id_note = @id_note";
                }
                else
                {
                    update = @"UPDATE notes SET 
                                note_name = @note_name, 
                                content = @content, 
                                image_url = @image_url, 
                                time_created = @time_created, 
                                pinned = @pinned,
                                id_user = @id_user, 
                                id_label = @id_label,  
                                id_status = @id_status 
                               WHERE id_note = @id_note";
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand(update, Connection))
                {
                    cmd.Parameters.AddWithValue("@id_note", note.IdNote);
                    cmd.Parameters.AddWithValue("@note_name", note.NoteName);
                    cmd.Parameters.AddWithValue("@content", note.Content);
                    cmd.Parameters.AddWithValue("@image_url", note.ImageUrl ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@time_created", note.TimeCreated);
                    cmd.Parameters.AddWithValue("@pinned", note.Pinned);
                    cmd.Parameters.AddWithValue("@id_user", note.IdUser);
                    cmd.Parameters.AddWithValue("@id_label", note.IdLabel ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@id_status", note.IdStatus);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Update failed! Error: " + ex.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }

        public bool DeleteNote(int idNote)
        {
            try
            {
                Connection.Open();
                string delete = @"DELETE FROM notes WHERE id_note = @id_note";
                using (NpgsqlCommand cmd = new NpgsqlCommand(delete, Connection))
                {
                    cmd.Parameters.AddWithValue("@id_note", idNote);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Delete failed! Error: " + ex.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }

        public bool UpdateIdLabel(int idNote, int? idLabel)
        {
            try
            {
                Connection.Open();
                string update = @"UPDATE notes SET id_label = @id_label WHERE id_note = @id_note";
                using (NpgsqlCommand cmd = new NpgsqlCommand(update, Connection))
                {
                    cmd.Parameters.AddWithValue("@id_note", idNote);
                    cmd.Parameters.AddWithValue("@id_label", idLabel.HasValue ? (object)idLabel.Value : DBNull.Value);
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBoxHelper.ShowErrorMessageBox("Update Label failed! Error: " + ex.Message);
                return false;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
