namespace EFSharpLab

open EFSharpLab.Db.Config
open EFSharpLab.Db.Config.Converters
open EFSharpLab.Models
open Microsoft.EntityFrameworkCore

type AppDbContext(options: DbContextOptions<AppDbContext>) =
    inherit DbContext(options)

    member this.Users = this.Set<User>()
    member this.Blogs = this.Set<Blog>()
    member this.Posts = this.Set<Post>()
    member this.Comments = this.Set<Comment>()

    override this.ConfigureConventions builder =
        block {
            builder.UseDatetimeUtc()
            builder.UseSingleCase<BlogId>()
            builder.UseSingleCase<PostId>()
            builder.UseSingleCase<UserId>()
            builder.UseSingleCase<CommentId>()
        }

    override this.OnModelCreating builder =
        base.OnModelCreating builder

        block {
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
            blog.HasOne<User>().WithMany().hasForeignKey (fun x -> x.OwnerId)

            blog.complexProperty (
                (fun x -> x.Meta),
                (fun b -> b.Property(fun x -> x.State).HasConversion<EntityStateConverter>())
            )

            let post = builder.Entity<Post>()
            post.hasKey (fun x -> x.Id)
            post.Property(fun x -> x.Title).HasMaxLength(50)
            post.Property(fun x -> x.Content)
            post.HasOne<Blog>().WithMany().hasForeignKey (fun x -> x.BlogId)
            post.HasOne<User>().WithMany().hasForeignKey (fun x -> x.AuthorId)
            post.Navigation(fun x -> x.Comments).AutoInclude()

            post.complexProperty (
                (fun x -> x.Meta),
                (fun b -> b.Property(fun x -> x.State).HasConversion<EntityStateConverter>())
            )

            let comment = builder.Entity<Comment>()
            comment.ToTable "Comment"
            comment.hasKey (fun x -> x.Id)
            comment.Property(fun x -> x.Text).HasMaxLength(120)
            comment.HasOne<User>().WithMany().hasForeignKey (fun x -> x.AuthorId)

            comment
                .HasOne<Post>()
                .WithMany(fun x -> x.Comments :> _ seq)
                .hasForeignKey (fun x -> x.PostId)

            comment.complexProperty (
                (fun x -> x.Meta),
                (fun b -> b.Property(fun x -> x.State).HasConversion<EntityStateConverter>())
            )

        }
