namespace Bennington.ContentTree.Providers.ContentNodeProvider.Helpers
{
    public interface IDetermineWhoHasTheAbilityToPublish
    {
        bool DetermineIfThisUserCanPublish(string username);
    }
}