(function () {
    const result = document.getElementById("result");
    const status = document.getElementById("status");
    const form = document.getElementById("custom-get");
    const pathInput = document.getElementById("custom-path");

    if (!result || !status) {
        return;
    }

    async function get(path) {
        if (pathInput) {
            pathInput.value = path;
        }

        status.textContent = "GET " + path;
        result.textContent = "Loading…";

        try {
            const response = await fetch(path, { headers: { Accept: "application/json" } });
            const text = await response.text();
            let body = text;
            try {
                body = JSON.stringify(JSON.parse(text), null, 2);
            } catch {
                // Keep the raw body when the response is not JSON.
            }

            status.textContent = "GET " + path + " → " + response.status + " " + response.statusText;
            result.textContent = body || "(empty)";
        } catch (error) {
            status.textContent = "GET " + path + " → failed";
            result.textContent = error instanceof Error ? error.message : String(error);
        }
    }

    document.querySelectorAll("button[data-path]").forEach(function (button) {
        button.addEventListener("click", function () {
            const path = button.getAttribute("data-path");
            if (path) {
                get(path);
            }
        });
    });

    if (form && pathInput) {
        form.addEventListener("submit", function (event) {
            event.preventDefault();
            const path = pathInput.value.trim();
            if (path) {
                get(path);
            }
        });
    }
})();
