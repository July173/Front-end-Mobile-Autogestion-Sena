using System.Collections.Generic;
using System;

namespace AutogestionSenaMaui.Helpers;

public static class RouteMap
{
    // Map of backend paths to Shell route names
    private static readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        { "/home", "MainDashboard" },
        { "home", "MainDashboard" },
        { "/request-registration", "NotImplemented" },
        { "request-registration", "NotImplemented" },
        { "/security", "SecurityMainPage" },
        { "security", "SecurityMainPage" },
        { "/admin", "AdminDashboard" },
        { "admin", "AdminDashboard" },
        { "/mass-registration", "NotImplemented" },
        { "mass-registration", "NotImplemented" },
        { "/reassign", "NotImplemented" },
        { "reassign", "NotImplemented" },
        { "/following", "NotImplemented" },
        { "following", "NotImplemented" },
        { "/following-history", "NotImplemented" },
        { "following-history", "NotImplemented" },
        { "/evaluate-final-visit", "NotImplemented" },
        { "evaluate-final-visit", "NotImplemented" },
        { "/assign", "NotImplemented" },
        { "assign", "NotImplemented" },
        { "/register-ep", "NotImplemented" },
        { "register-ep", "NotImplemented" },
        { "/application-evaluation", "NotImplemented" },
        { "application-evaluation", "NotImplemented" },
        // Add additional mappings here, e.g.:
        // { "/users", "SecurityUsers" },
        // { "users", "SecurityUsers" },
    };

    public static string? Map(string? backendPath)
    {
        if (string.IsNullOrEmpty(backendPath))
            return null;

        // Normalize path: remove starting slash
        var key = backendPath.Trim().TrimStart('/');
        if (string.IsNullOrEmpty(key))
            return null;

        if (_map.TryGetValue(backendPath, out var route))
            return route;

        if (_map.TryGetValue(key, out route))
            return route;

        // Try to match using last segment
        var segments = key.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length > 0 && _map.TryGetValue(segments.Last(), out route))
            return route;

        return null; // Not found
    }

    public static void Register(string backendPath, string shellRoute)
    {
        if (string.IsNullOrWhiteSpace(backendPath) || string.IsNullOrWhiteSpace(shellRoute))
            return;

        var key = backendPath.Trim().TrimStart('/');
        _map[key] = shellRoute;
        _map[backendPath.Trim()] = shellRoute;
    }

    public static void LoadFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return;

        try
        {
            var dict = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (dict == null) return;

            foreach (var kv in dict)
            {
                Register(kv.Key, kv.Value);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[RouteMap] Error parsing JSON: {ex}");
        }
    }
}
