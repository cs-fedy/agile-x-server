using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class CommentErrors
{
    public static Error CommentNotFound = Error.NotFound(
        code: "Comment.NotFound",
        description: "Requested comment not found"
    );

    public static Error NotCommentOwner = Error.Unauthorized(
        code: "Comment.NotOwner",
        description: "User is not authorized to perform "
            + "actions on this comment as he is not the owner"
    );
}
