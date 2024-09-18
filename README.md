
# MVA_poe - Civic Connect

## Overview

The MVA_poe (also known as Civic Connect) application is a C# WPF application developed for municipal use. It aims to facilitate citizen engagement by enabling users to report issues and service requests efficiently. The application is designed to be user-friendly, providing a seamless experience for residents to interact with municipal services.

## Features

- **User Registration**: Required on startup.
- **Create New Report**: Users can submit details about various issues, including location and category. Image and document attachments are supported.
- **Local Events and Announcements**: To be implemented.
- **Service Request Status**: To be implemented.

## Usage

### User Account

- **Register an Account**: On application startup, users must register by providing their name, surname, ID, address, and password. If the email address is not already registered, an account will be created.
- **Login**: Users log in with their ID and password.

### Creating Reports

- **Launch the Application**: After login, navigate to the "Create New Report" section via the dashboard or navigation bar.
- **Enter Report Details**:
  - **Location**: Specify the issue's location.
  - **Description**: Describ the issue.
  - **Category**: Choose the issue category.
  - **Attachments**: Optionally attach images or documents related to the issue.
- **Submit Report**: Click "Submit" to save the report.

### Viewing Reports

- **Navigate to "View Report"**: Access this feature via the navigation bar.
- **Select Report**: Choose the desired report from the dropdown menu to view details.

## Application Behavior

- **Data Storage**: The application uses a local database stored within the app folder for data storage. User information, including securely hashed passwords, is stored in this local database to ensure reliability and security.
- **User Registration and Password Management**: User passwords are securely hashed before storage.
- **Exiting the Application**: Users can log out or exit the application securely.

## References

- Payload (2021). WPF C# Professional Modern Flat UI Tutorial. Available at: [YouTube](https://www.youtube.com/watch?v=PzP8mw7JUzI&t=1946s) (Accessed: 9 September 2024).
- CodeCraks (2023). How to Create a Modern Splash Screen in WPF using C#. Available at: [YouTube](https://www.youtube.com/watch?v=XM_I1y1mh7k&t=2s) (Accessed: 16 September 2024).
- Coding Under Pressure (2020). How to Let User Browse and Upload Files in WPF C#. Available at: [YouTube](https://www.youtube.com/watch?v=DKYssZ8JUx0) (Accessed: 16 September 2024).
- Jd’s Code Lab (2021). WPF C# | Drag & Drop to Upload File UI. Available at: [YouTube](https://www.youtube.com/watch?v=eEa_Fl3ZguA&t=2s) (Accessed: 16 September 2024).
- Jeyderht (2021). C# WPF - Modern Vertical Menu. Available at: [YouTube](https://www.youtube.com/watch?v=Et-QcvwKzY4&t=4s) (Accessed: 16 September 2024).
- Jeyderht (2021). Jeyderht/WPFMODERNVERTICALMENU. Available at: [GitHub](https://github.com/Jeyderht/WPFModernVerticalMenu) (Accessed: 16 September 2024).
- OpenAI. 2024. ChatGPT (Version 4.0). [Large language model]. Available at:(https://chat.openai.com/](https://chatgpt.com/share/66eb1daa-a24c-800b-984a-39a1191c949c). (Accessed: 18 September 2024).

## License

The MIT License (MIT)

Copyright (c) 2024 Hannah Michaelson

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

