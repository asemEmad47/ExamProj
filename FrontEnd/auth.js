// Refresh the access token if expired
async function refreshToken() {
    try {
        const refreshToken = localStorage.getItem("refreshToken");
        if (!refreshToken) throw new Error("No refresh token found");

        const response = await fetch("https://localhost:7216/api/Auth/Refresh", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ RefreshToken: refreshToken }) // Updated to match backend expectation
        });

        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(`Failed to refresh token: ${response.status} - ${JSON.stringify(errorData)}`);
        }

        const data = await response.json();
        localStorage.setItem("accessToken", data.accessToken);
        localStorage.setItem("refreshToken", data.refreshToken);
        console.log("Token refreshed successfully:", data);
        return data.accessToken;
    } catch (error) {
        console.error("Error refreshing token:", error);
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        alert("Session expired. Please log in again.");
        return null;
    }
}




async function apiCall(url, options = {}) {
    try {
        let accessToken = localStorage.getItem("accessToken");

        if (!options.headers) options.headers = {};
        options.headers["Authorization"] = "Bearer " + accessToken;

        let response = await fetch(url, options);

        // Handle token expiration
        if (response.status === 401) {
            const newToken = await refreshToken();
            if (!newToken) throw new Error("Unable to refresh token");

            // Retry the original request with the new token
            options.headers["Authorization"] = "Bearer " + newToken;
            response = await fetch(url, options);
        }

        // Check for other errors
        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(`Request failed: ${response.status} - ${errorText}`);
        }

        // Check if the response contains JSON data
        const contentType = response.headers.get("content-type");
        if (contentType && contentType.includes("application/json")) {
            return await response.json();
        }

        // Return an empty object if no JSON data is present
        return {};
    } catch (error) {
        console.error("API call error:", error);
        throw error;
    }
}
