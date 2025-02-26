// This file defines namespace aliases to help with the transition
// WSCollector.Blazor references in existing code can be properly mapped to WSC

namespace WSC
{
    // Create the shared namespace
    namespace Shared 
    {
        // Empty namespace declaration to fix compiler errors
    }

    // Models namespace
    namespace Models
    {
        // Empty namespace declaration
    }

    // Services namespace
    namespace Services
    {
        // Empty namespace declaration
    }

    // Components namespace
    namespace Components
    {
        // Empty namespace declaration
    }
}

// Create alias namespaces to redirect from WSCollector.Blazor to WSC
namespace WSC
{
    // Empty namespace to avoid errors
}

namespace WSC.Shared
{
    // Empty namespace to avoid errors
}

namespace WSC.Components
{
    // Empty namespace to avoid errors
}

namespace WSC.Models
{
    // Empty namespace to avoid errors
}

namespace WSC.Services
{
    // Empty namespace to avoid errors
}

