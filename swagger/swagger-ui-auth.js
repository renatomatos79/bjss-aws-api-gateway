function buildUI() {
    // Begin Swagger UI call region
    const ui = SwaggerUIBundle({
        urls: [
            { url: "https://5osboavjwl.execute-api.us-east-1.amazonaws.com/dev/swagger/login", name: "Auth" },
            { url: "https://5osboavjwl.execute-api.us-east-1.amazonaws.com/dev/swagger/events", name: "Events" }
        ],
        dom_id: '#swagger-ui',
        deepLinking: true,
        presets: [
            SwaggerUIBundle.presets.apis,
            SwaggerUIStandalonePreset
        ],
        plugins: [
            SwaggerUIBundle.plugins.DownloadUrl
        ],
        layout: "StandaloneLayout"
    })
    // End Swagger UI call region

    window.ui = ui
}

function disableButton(btn, isDisabled) {
    btn.disabled = isDisabled;
    btn.class = isDisabled ? 'disabled-btn' : '';
}

function fetchToken(swaggerUI, modal, loginForm, email, password, loginBtn) {
    const url = 'https://5osboavjwl.execute-api.us-east-1.amazonaws.com/dev/login';
    const payload = {
        "userName": email,
        "password": password
    };
    
    // Make the POST request
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload) // Convert the payload to JSON
    })
        .then(response => {
            disableButton(loginBtn, false)
            
            if (response.status !== 200) {
                alert('Não foi possível autenticar o usuário. Verifique os dados e tente novamente!');
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json(); // Parse JSON response
        })
        .then(data => {
            // Hide the modal after submission
            modal.style.display = 'none';

            // Display the Swagger UI
            swaggerUI.style.display = 'flex';
            
            // Clear form fields
            loginForm.reset();
            
            buildUI();
        })
        .catch(error => {
            console.error('Error:', error); // Handle error
        });
}


window.onload = function () {
    const swaggerUI = document.getElementById('swagger-ui');
    const modal = document.getElementById('modal');
    const loginForm = document.getElementById('loginForm');
    const loginBtn = document.getElementById('loginBtn');

    // display the modal
    modal.style.display = 'flex';

    window.addEventListener('click', (event) => {
        if (event.target.id === 'closeModalBtn') {
            modal.style.display = 'none';
        }
    });

    // Handle form submission
    loginForm.addEventListener('submit', (event) => {
        event.preventDefault(); // Prevent page reload on form submission

        // Disable the login button
        disableButton(loginBtn, true)

        // Get username and password values
        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        // Build the Swagger UI
        fetchToken(swaggerUI, modal, loginForm, username, password, loginBtn);
    });


}
