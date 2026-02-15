namespace UniversityERP.Infrastructure.EmailTemplates;

public static class UserEmails
{
    public static string Credentials(string fullName, string uniEmail, string tempPassword)
        => $@"
<h2>University Account Created</h2>
<p>Hello <b>{System.Net.WebUtility.HtmlEncode(fullName)}</b>,</p>
<p>Your university account is ready.</p>
<p>
<b>Login email:</b> {System.Net.WebUtility.HtmlEncode(uniEmail)}<br/>
<b>Password:</b> {System.Net.WebUtility.HtmlEncode(tempPassword)}
</p>
<p>You can change your password anytime after login.</p>";

    public static string PasswordReset(string fullName, string newPassword)
        => $@"
<h2>Password Reset</h2>
<p>Hello <b>{System.Net.WebUtility.HtmlEncode(fullName)}</b>,</p>
<p>Your password was reset by the university administration.</p>
<p><b>New password:</b> {System.Net.WebUtility.HtmlEncode(newPassword)}</p>
<p>If you did not expect this, contact support.</p>";
}