namespace FSharpWithEF

open FSharpWithEF.Db.Config
open FSharpWithEF.Db.Config.Converters
open FSharpWithEF.Models
open Microsoft.EntityFrameworkCore

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    member this.Users = this.Set<User>()
    member this.Blogs = this.Set<Blog>()

    override this.ConfigureConventions builder =
        proc {
            builder.UseDatetimeUtc()
            builder.UseSingleCase<BlogId>()
            builder.UseSingleCase<PostId>()
            builder.UseSingleCase<UserId>()
            builder.UseSingleCase<CommentId>()
        }

    override this.OnModelCreating builder =
        base.OnModelCreating builder

        proc {
            let user = builder.Entity<User>()
            user.ToTable "User"
            user.hasKey (fun x -> x.Id)
            user.Property(fun x -> x.FirstName).HasMaxLength(50)
            user.Property(fun x -> x.LastName).HasMaxLength(100).IsOptional()
            user.Property(fun x -> x.Gender).HasConversion<GenderConverter>()

            user.complexProperty (
                (fun x -> x.Meta),
                (fun b -> b.Property(fun x -> x.State).HasConversion<EntityStateConverter>())
            )

            let blog = builder.Entity<Blog>()
            blog.hasKey (fun x -> x.Id)
            blog.Property(fun x -> x.Title).HasMaxLength(40)
            blog.Navigation(fun x -> x.Owner).AutoInclude()
            // blog.hasMany(fun x -> x.Posts).WithMany()
            blog.complexProperty (
                (fun x -> x.Meta),
                (fun b -> b.Property(fun x -> x.State).HasConversion<EntityStateConverter>())
            )
        }
