using System.Data.SQLite;
using System.Web.Configuration;

namespace AspNetMvc5WithSqlite
{
    public static class SqliteInitializer
    {
        public static void Initialize()
        {
            using (
                var con =
                    new SQLiteConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString)
                )
            {
                con.Open();

                var com = new SQLiteCommand(con);

                com.CommandText = @"
CREATE TABLE IF NOT EXISTS [AspNetRoles] (
    [Id]   NVARCHAR (128) NOT NULL,
    [Name] NTEXT NOT NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id] ASC)
);


CREATE TABLE IF NOT EXISTS [AspNetUserClaims] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [ClaimType]  NText NULL,
    [ClaimValue] NTEXT NULL,
    [User_Id]    NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY  ([Id] ASC),
    CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_User_Id] FOREIGN KEY ([User_Id]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS [IX_AspNetUserClaims_User_Id]
    ON [AspNetUserClaims]([User_Id] ASC);


CREATE TABLE IF NOT EXISTS [AspNetUserLogins] (
    [UserId]        NVARCHAR (128) NOT NULL,
    [LoginProvider] NVARCHAR (128) NOT NULL,
    [ProviderKey]   NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([UserId] ASC, [LoginProvider] ASC, [ProviderKey] ASC),
    CONSTRAINT [FK_AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS [IX_UserId]
    ON [AspNetUserLogins]([UserId] ASC);


CREATE TABLE IF NOT EXISTS [AspNetUserRoles] (
    [UserId] NVARCHAR (128) NOT NULL,
    [RoleId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY  ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS [IX_RoleId]
    ON [AspNetUserRoles]([RoleId] ASC);

CREATE INDEX IF NOT EXISTS [IX_AspNetUserRoles_UserId]
    ON [AspNetUserRoles]([UserId] ASC);


CREATE TABLE IF NOT EXISTS [AspNetUsers] (
    [Id]            NVARCHAR (128) NOT NULL,
    [UserName]      NTEXT NULL,
    [PasswordHash]  NTEXT NULL,
    [SecurityStamp] NTEXT NULL,
    [Discriminator] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id] ASC)
);


CREATE TABLE IF NOT EXISTS [__MigrationHistory] (
  [MigrationId] NVARCHAR(150) NOT NULL, 
  [ContextKey] nvARCHAR(300) NOT NULL, 
  [Model] NTEXT NOT NULL, 
  [ProductVersion] nvARCHAR(32) NOT NULL, 
  CONSTRAINT [] PRIMARY KEY ([MigrationId] ASC, [ContextKey] ASC));

";
                com.ExecuteNonQuery();

            }

        }
    }
}