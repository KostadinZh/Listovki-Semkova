using ListovkiQuiz.Services;

namespace ListovkiQuiz.Tests;

/// <summary>
/// Tests password hashing and verification to ensure passwords are stored and checked securely.
/// </summary>
public class PasswordsTests
{
    [Fact]
    public void Verify_AcceptsTheCorrectPassword_AndRejectsAWrongOne()
    {
        
        string saved = Passwords.Hash("parola123");

          // Only the original password should match the stored hash.
        Assert.True(Passwords.Verify("parola123", saved));
        Assert.False(Passwords.Verify("parola124", saved));
    }

    [Fact]
    public void Hash_IsDifferentEveryTime_BecauseOfTheRandomSalt()
    {
       
        string first = Passwords.Hash("parola123");
        string second = Passwords.Hash("parola123");

       // A new random salt should produce a different hash for the same password.
        Assert.NotEqual(first, second);

         // Both hashes must still be valid for the original password.
        Assert.True(Passwords.Verify("parola123", first));
        Assert.True(Passwords.Verify("parola123", second));
    }
}
