using BicycleCompany.DAL;
using BicycleCompany.DAL.Models;
using System;
using System.Collections.Generic;

namespace BicycleCompany.IntegrationTests.Utils
{
    public static class DbManager
    {
        public static void InitializeDbForTests(RepositoryContext db)
        {
            db.Users.AddRange(GetSeedingUsers());
            db.Bicycles.AddRange(GetSeedingBicycles());
            db.Clients.AddRange(GetSeedingClient());

            var parts = GetSeedingParts();
            db.Parts.AddRange(parts);
            db.Problems.AddRange(GetSeedingProblems(parts));

            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(RepositoryContext db)
        {
            db.Users.RemoveRange(GetSeedingUsers());
            db.Bicycles.RemoveRange(GetSeedingBicycles());
            db.Clients.RemoveRange(GetSeedingClient());
            db.Parts.RemoveRange(GetSeedingParts());
            db.Problems.RemoveRange(GetSeedingProblems());

            InitializeDbForTests(db);
        }

        public static User[] GetSeedingUsers()
        {
            return new User[]
            {
                new User
                {
                    Id = new Guid("5A17C699-6942-46C6-91EB-4E9843A186B0"),
                    Login = "Admin1",
                    Password = "7aekCVlgVr2mHBSiG7j4oYFRcuVvuQpsx/LGoBEn+WY=", // Admin
                    Salt = "U+c7ldHlOzDGQwkVtbo4rQ==",
                    Role = "Administrator"
                },
                new User
                {
                    Id = new Guid("5A17C699-6942-46C6-91EB-4E9843A186B1"),
                    Login = "User1",
                    Password = "Ugu85msDktPCb+4dq2eH9178FcPJPiJ1GoZDuVKvdI8=", // User
                    Salt = "UZ87zCZbv7Xn1nh7n1riYQ==",
                    Role = null
                },
                new User
                {
                    Id = new Guid("5A17C699-6942-46C6-91EB-4E9843A186B2"),
                    Login = "Master1",
                    Password = "Bf/97pp16vaCipEI2w/LM1P1XcP7WKVmSIT9XmpnbOo=", // Master
                    Salt = "N1UVkH2kwLrs6aoEADLuGg==",
                    Role = "Master"
                },
                new User
                {
                    Id = new Guid("5A17C699-6942-46C6-91EB-4E9843A186B3"),
                    Login = "Manager1",
                    Password = "wjPHtXpNvhueKzcqH+dgfLG1Lfi/EpuYqARC/p9T25c=", // Manager
                    Salt = "vm8TTiETaZAroOITxE6yJw==",
                    Role = "Manager"
                }
            };
        }

        public static Problem[] GetSeedingProblems(List<Part> parts = null)
        {
            return new Problem[]
            {
                new Problem
                {
                    Id = new Guid("E3F5E0B1-C8BD-4405-87E4-9F417DD54C20"),
                    ClientId = new Guid("9EE78239-C83B-4B4C-BAA3-793A1906AA80"),
                    BicycleId = new Guid("44D8EDA9-BB8C-4339-97C0-20B9A186F0A0"),
                    Place = "Place 0",
                    Stage = Stage.New,
                    ReceivingDate = new DateTime(2021, 7, 12),
                    Description = "Description 0"
                },
                new Problem
                {
                    Id = new Guid("E3F5E0B1-C8BD-4405-87E4-9F417DD54C21"),
                    ClientId = new Guid("9EE78239-C83B-4B4C-BAA3-793A1906AA81"),
                    BicycleId = new Guid("44D8EDA9-BB8C-4339-97C0-20B9A186F0A1"),
                    Place = "Place 1",
                    Stage = Stage.New,
                    ReceivingDate = new DateTime(2021, 7, 13),
                    Description = "Description 1",
                    Parts = parts
                }
            };
        }

        public static List<Part> GetSeedingParts()
        {
            return new List<Part>()
            {
                new Part
                {
                    Id = new Guid("C19B07D1-BCD8-4CDE-88C0-47CCBD3CC1B0"),
                    Name = "Part1"
                },
                new Part
                {
                    Id = new Guid("C19B07D1-BCD8-4CDE-88C0-47CCBD3CC1B1"),
                    Name = "Part2"
                },
                new Part
                {
                    Id = new Guid("C19B07D1-BCD8-4CDE-88C0-47CCBD3CC1B2"),
                    Name = "Part3"
                }
            };
        }

        public static Client[] GetSeedingClient()
        {
            return new Client[]
            {
                new Client
                {
                    Id = new Guid("9EE78239-C83B-4B4C-BAA3-793A1906AA80"),
                    Name = "Client1"
                },
                new Client
                {
                    Id = new Guid("9EE78239-C83B-4B4C-BAA3-793A1906AA81"),
                    Name = "Client2"
                }
            };
        }

        public static Bicycle[] GetSeedingBicycles()
        {
            return new Bicycle[]
            {
                new Bicycle
                {
                    Id = new Guid("44D8EDA9-BB8C-4339-97C0-20B9A186F0A0"),
                    Name = "Bicycle1",
                    Model = "Model1"
                },
                new Bicycle
                {
                    Id = new Guid("44D8EDA9-BB8C-4339-97C0-20B9A186F0A1"),
                    Name = "Bicycle2",
                    Model = "Model2"
                },
                new Bicycle
                {
                    Id = new Guid("44D8EDA9-BB8C-4339-97C0-20B9A186F0A2"),
                    Name = "Bicycle3",
                    Model = "Model3"
                }
            };
        }
    }
}
