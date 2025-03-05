/* prompts:
1. When the form is submitted, store the username, password, and the current date and time in an array.
2. Log all login attempts to the console.
3. Display the current time on the page.    
4. When the 'h' key is pressed, toggle the visibility of the login form.
*/



// Array to store login attempts
const loginAttempts = [];

// Wait for the DOM to fully load before running the script
document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.querySelector('.login-form');
    const loginContainer = document.querySelector('.login-container');
    
    loginForm.addEventListener('submit', (e) => {
       
        e.preventDefault();
        
        
        const vikingName = document.querySelector('input[type="text"]').value;
        const password = document.querySelector('input[type="password"]').value;
        
        // Create an object to store the login attempt details
        const attempt = {
            username: vikingName,
            password: password,
            timestamp: new Date().toLocaleString() // Record the current date and time
        };
        
        // Add the login attempt to the array
        loginAttempts.push(attempt);
        
        // Log all login attempts to the console
        console.log('🗡️ Viking Login Attempts:', loginAttempts);
        
    
        loginForm.reset();
    });

    // Function to update the clock display
    function updateClock() {
        const clockElement = document.getElementById('clock');
        const now = new Date();
        const timeString = now.toLocaleTimeString(); // Get the current time as a string
        clockElement.textContent = timeString; // Update the clock element with the current time
    }

    // Update the clock every second
    setInterval(updateClock, 1000);
    updateClock(); 

    // Function to toggle the visibility of the login form
    function toggleFormsVisibility() {
        if (loginContainer.style.visibility === 'hidden') {
            loginContainer.style.visibility = 'visible';
            loginContainer.style.opacity = '1';
        } else {
            loginContainer.style.visibility = 'hidden';
            loginContainer.style.opacity = '0';
        }
    }

    // Add event listener for key press to toggle form visibility
    document.addEventListener('keydown', (event) => {
        if(document.activeElement.tagName === 'INPUT') {
            return; 
        }
        if (event.key === 'h' || event.key === 'H') {
            toggleFormsVisibility(); // Toggle the form visibility when 'h' or 'H' is pressed
        }
    });
});