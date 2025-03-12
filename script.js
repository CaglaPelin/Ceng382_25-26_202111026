/* prompts:
1. Write JavaScript code to redirect to "table.html" when the username and password are "admin".
2. Store successful login attempts in an array.
3. Implement a form in "table.html" to add class details (Class Name, Number of People, Description).
4. Dynamically update the table with submitted class details without reloading the page.
5. Add JavaScript event listeners:
   - Mouseover Event: Change the background color when hovering over a table row.
   - Mouseout Event: Restore the background color when the mouse leaves the row.
    - Click Event: When a row in the table is clicked, log its details to the console.
    - Table Click Event: When the table is clicked (outside individual rows), an array containing all class entries is printed to the console.
6. Display a real-time clock inside an HTML element with the ID "clock".
   - Ensure the "clock" element exists before updating its text.
7. Implement a key press event to toggle form visibility when pressing 'H'.
8. Add an alert functionality to alert when user presses a class name. Alert will show the classes informations
*/



// Arrays to store login attempts
const loginAttempts = [];
const successfulLogins = [];

document.addEventListener('DOMContentLoaded', () => {
    const loginForm = document.querySelector('.login-form');
    const loginContainer = document.querySelector('.login-container');
    const classForm = document.getElementById('class-form');
    const classTableBody = document.querySelector('#class-table tbody');
    const classTable = document.getElementById('class-table');
    const rowDetails = document.getElementById('row-details'); 

    if (loginForm) {
        loginForm.addEventListener('submit', (event) => {
            event.preventDefault();
            
            const username = loginForm.querySelector('input[type="text"]').value;
            const password = loginForm.querySelector('input[type="password"]').value;
            
            const loginAttempt = {
                username: username,
                password: password,
                timestamp: new Date().toLocaleString(),
                success: false
            };
            
            if (username === 'admin' && password === 'admin') {
                loginAttempt.success = true;
                successfulLogins.push(loginAttempt);
                
                console.log('Previous Login Attempts:');
                console.table(loginAttempts);
                console.log('------------------------');
                
                loginAttempts.push(loginAttempt);
                
                console.log('All Login Attempts (Including Current):');
                console.table(loginAttempts);
                console.log('------------------------');
                
                console.log('Successful Logins:');
                console.table(successfulLogins);
                console.log('------------------------');
                
                window.location.href = 'table.html';
            } else {
                alert('Invalid credentials. Please try again.');
                loginAttempts.push(loginAttempt);
                
                console.log('All Login Attempts:');
                console.table(loginAttempts);
                console.log('------------------------');
            }
        });
    }

    if (classForm) {
        classForm.addEventListener('submit', (event) => {
            event.preventDefault();

            const className = document.getElementById('class-name').value;
            const numberOfPeople = document.getElementById('number-of-people').value;
            const description = document.getElementById('description').value;

            const newRow = document.createElement('tr');
            newRow.innerHTML = `
                <td class="class-name">${className}</td>
                <td>${numberOfPeople}</td>
                <td>${description}</td>
            `;

            classTableBody.appendChild(newRow);

            
            addRowHoverEffect(newRow);

            
            addRowClickEvent(newRow);

            classForm.reset();
        });
    }

    // Function to update the clock display
    function updateClock() {
        const clockElement = document.getElementById('clock');
        if (clockElement) {
            const now = new Date();
            const timeString = now.toLocaleTimeString(); 
            clockElement.textContent = timeString; 
        }
    }

    setInterval(updateClock, 1000);
    updateClock(); 

    // Function to toggle form visibility
    function toggleFormsVisibility() {
        if (loginContainer.style.visibility === 'hidden') {
            loginContainer.style.visibility = 'visible';
            loginContainer.style.opacity = '1';
        } else {
            loginContainer.style.visibility = 'hidden';
            loginContainer.style.opacity = '0';
        }
    }

    // Keyboard event: Press 'H' to toggle form visibility
    document.addEventListener('keydown', (event) => {
        if (document.activeElement.tagName === 'INPUT') {
            return; 
        }
        if (event.key === 'h' || event.key === 'H') {
            toggleFormsVisibility(); 
        }
    });

    // Function to add mouseover/mouseout effects to table rows
    function addRowHoverEffect(row) {
        row.addEventListener('mouseover', () => {
            row.style.background = 'linear-gradient(145deg, #4a4e54, #2a2d30)';
            row.style.transform = 'translateX(10px)';
            row.style.transition = 'all 0.3s ease';
            row.style.color = '#ffffff';
            row.style.boxShadow = '0 0 15px rgba(255, 255, 255, 0.1)';
            
            const firstCell = row.querySelector('td:first-child');
            if (firstCell) {
                firstCell.setAttribute('data-before', '⚔️');
                firstCell.style.position = 'relative';
                firstCell.style.textShadow = '0 0 10px rgba(255, 255, 255, 0.3)';
            }

            const cells = row.querySelectorAll('td');
            cells.forEach(cell => {
                cell.style.borderColor = '#4a4e54';
                cell.style.transition = 'all 0.3s ease';
                cell.style.backgroundColor = 'rgba(42, 45, 48, 0.9)';
            });
        });

        row.addEventListener('mouseout', () => {
            row.style.background = '';
            row.style.transform = '';
            row.style.color = '';
            row.style.boxShadow = '';
            
            const firstCell = row.querySelector('td:first-child');
            if (firstCell) {
                firstCell.removeAttribute('data-before');
                firstCell.style.textShadow = '';
            }

            const cells = row.querySelectorAll('td');
            cells.forEach(cell => {
                cell.style.borderColor = '';
                cell.style.transition = '';
                cell.style.backgroundColor = '';
            });
        });
    }

    // Function to add click event to table rows
    function addRowClickEvent(row) {
        row.addEventListener('click', (event) => {
            const cells = row.querySelectorAll('td');
            const rowData = Array.from(cells).map(cell => cell.textContent);
            console.log('Row Data:', rowData);

            
            row.style.background = 'linear-gradient(145deg, #2a2d30, #1a1d20)';
            row.style.color = '#ffffff';
            row.style.boxShadow = '0 0 20px rgba(255, 255, 255, 0.2)';

            
            if (rowDetails) {
                rowDetails.innerHTML = `
                    <p><strong>Class Name:</strong> ${rowData[0]}</p>
                    <p><strong>Number of People:</strong> ${rowData[1]}</p>
                    <p><strong>Description:</strong> ${rowData[2]}</p>
                `;
            }

            // Show alert with class information for any click on the row
            alert(`Class Name: ${rowData[0]}\nNumber of People: ${rowData[1]}\nDescription: ${rowData[2]}`);
        });
    }

    // Apply hover and click events to existing rows
    const tableRows = document.querySelectorAll('#class-table tbody tr');
    tableRows.forEach(row => {
        addRowHoverEffect(row);
        addRowClickEvent(row);
    });

    // Table click event to log all class entries
    if (classTable) {
        classTable.addEventListener('click', (event) => {
            if (event.target.tagName !== 'TD') {
                const allRows = classTableBody.querySelectorAll('tr');
                const allData = Array.from(allRows).map(row => {
                    const cells = row.querySelectorAll('td');
                    return Array.from(cells).map(cell => cell.textContent);
                });
                console.log('All Class Entries:', allData);

                // Display all class entries in the new container
                if (rowDetails) {
                    rowDetails.innerHTML = allData.map(data => `
                        <p><strong>Class Name:</strong> ${data[0]}</p>
                        <p><strong>Number of People:</strong> ${data[1]}</p>
                        <p><strong>Description:</strong> ${data[2]}</p>
                    `).join('');
                }
            }
        });
    }
});
