using DogGo.Models;
using Microsoft.Data.SqlClient;

namespace DogGo.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly IConfiguration _config;

        public WalkRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Walk> GetWalksByWalker(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using(SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                        Select w.Id,
                                               w.Date, 
		                                       o.Name AS Client,
		                                       w.Duration,
                                               w.WalkerId,
                                               w.DogId
                                        From Walks w
                                        Left Join Dog d on d.Id = w.DogId
                                        Left Join Owner o on o.Id = d.OwnerId
                                        Where WalkerId = @id
                                      ";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Walk> walks = new List<Walk>();
                    while (reader.Read())
                    {
                        Walk walk = new Walk()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                            Duration = (reader.GetInt32(reader.GetOrdinal("Duration")))/60,
                            WalkerId = reader.GetInt32(reader.GetOrdinal("WalkerId")),
                            DogId = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Client = reader.GetString(reader.GetOrdinal("Client"))
                        };
                        walks.Add(walk);
                    }
                    reader.Close();
                    return walks;

                }
            }
        }
    }
}
