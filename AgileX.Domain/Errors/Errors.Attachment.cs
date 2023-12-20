using AgileX.Domain.Result;

namespace AgileX.Domain.Errors;

public static class AttachmentErrors
{
    public static Error AttachmentNotFound = Error.NotFound(
        code: "Attachment.NotFound",
        description: "Requested Attachment not found"
    );

    public static Error InvalidAccessAttachment = Error.Unauthorized(
        code: "Attachment.InvalidAccess",
        description: "Invalid access to requested attachment"
    );
}
