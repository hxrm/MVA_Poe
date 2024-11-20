
# MVA_poe - Civic Connect

## Login Credentials
- **Email**: demo@gmail.com
- **Password**: Shalom78*

## Overview

The MVA_poe (also known as Civic Connect) application is a C# WPF application developed for municipal use. It aims to facilitate citizen engagement by enabling users to report issues and service requests efficiently. The application is designed to be user-friendly, providing a seamless experience for residents to interact with municipal services.

## Features

- **User Registration**: Required on startup.
- **Create New Report**: Users can submit details about various issues, including location and category. Image and document attachments are supported.
- **Local Events and Announcements**: Allows users to view and search for events, receive personalized recommendations, and filter events by category or date.
- **Service Request Status**: Allows users to view and track the status of service requests and track progress over time.

## Installation Instructions

### Clone the Project from GitHub

1. **Clone the Repository**:  
   Open your terminal (or command prompt) and run the following command:
   ```bash
   git clone  https://github.com/ST10158643/MVA_Poe.git
   ```

2. **Navigate to the Project Directory**:  
   Once the repository is cloned, navigate to the project directory:
   ```bash
   cd MVA_poe
   ```

3. **Open the Project**:  
   Open the project in your preferred IDE (e.g., Visual Studio, Rider, or Visual Studio Code).

4. **Install Dependencies**:  
   If there are any dependencies listed in the project, install them via your IDE's package manager.

5. **Build the Application**:  
   Build the project within your IDE to ensure all configurations are correct.
---
---

### Download the Application Project as a ZIP Folder

1. **Download the ZIP File**:  
   Go to the [GitHub Repository](https://github.com/ST10158643/MVA_Poe) and click on the **Code** button. Select **Download ZIP**.

2. **Extract the ZIP File**:  
   Once downloaded, extract the ZIP file to a folder on your local machine.

3. **Open the Project**:  
   Open the extracted folder in your preferred IDE (e.g., Visual Studio, Rider, or Visual Studio Code).

4. **Install Dependencies**:  
   Ensure that all required dependencies are correctly installed. The project may require certain libraries or NuGet packages. Visual Studio will automatically manage this through the NuGet Package Manager.

5. **Build the Application**:  
   Build the project within your IDE to ensure all configurations are correct.

---

### Compile and Run the Application

After downloading and installing the application, here’s how to compile and run it:

1. **Open the Project in Visual Studio**:
   - If you don’t have Visual Studio, download and install it from [here](https://visualstudio.microsoft.com/downloads/).
   - Open the downloaded project or files in Visual Studio.

2. **Check Dependencies**:  
   Ensure that all required dependencies are correctly installed. The project may require certain libraries or NuGet packages. Visual Studio will automatically manage this through the NuGet Package Manager.

3. **Compile the Application**:
   - In Visual Studio, click on the **Build** menu.
   - Select **Build Solution** (or press `Ctrl + Shift + B`).
   - This will compile the application and ensure there are no errors.

4. **Run the Application**:
   - Once the build is successful, click **Start** (green play button) or press `F5` to run the application.
   - The application will open, and you can start using it as described in the features section.

---

### Creating Reports

- **Launch the Application**: After login, navigate to the "Create New Report" section via the dashboard or navigation bar.
- **Enter Report Details**:
  - **Location**: Specify the issue's location.
  - **Description**: Describe the issue.
  - **Category**: Choose the issue category.
  - **Attachments**: Optionally attach images or documents related to the issue.
- **Submit Report**: Click "Submit" to save the report.

### Viewing Reports

- **Navigate to "View Report"**: Access this feature via the navigation bar.
- **Select Report**: Choose the desired report from the dropdown menu to view details.
  
### Local Events and Announcements

Users can access the "Local Events and Announcements" section from the Dashboard home screen via the navigation bar. Upon selection, they will be presented with a list of all events.
- **Navigate to the Local Events and Announcements Section:**: After logging in, go to the Dashboard and select Local Events and Announcements from the navigation bar.
- **View Events**: Users can click on an event to view full details, including time, location, and description.
- **Search Feature**: Users can search for events by entering a keyword related to the event name or category. To reset the search and view all events again, click the "Reset" button.
- **Advanced Search**: Clicking the filter button next to the reset option opens an advanced search panel. This allows users to refine their search by selecting a category from a dropdown and specifying a date range using a calendar.
- **Personalized Recommendations**: The application will suggest events based on your past search patterns. These recommendations will be shown after you perform your first search. The system may also suggest related events from other categories associated with your preferences.


### Service Request Status

To track the status of a service requests, follow these steps:

1. **Navigate to the Service Request Status Section**:  
   After logging in, go to the **Dashboard** and select **Service Request Status** from the navigation bar.

2. **View Service Requests**:  
   A list of all your submitted service requests will be displayed. Each request includes:
   - **Unique Identifier (ID)**: A unique ID for each service request.
   - **Current Status**: The current status of the request, such as **Pending**, **In Progress**, or **Completed**.
   - **Priority**: The priority of the service request (e.g., Low, Medium, High) is also displayed to help identify the urgency of each request.

3. **Track Progress**:  
   - Each service request will show its progress alongside its status (e.g., Pending, In Progress, Completed). This allows you to easily track the progress of any open requests.

4. **Search and Filter**:  
   - **Search by ID**: Use the search bar to search for service requests by their unique identifier (ID).
   - **Filter by Status**: You can filter service requests by their current status (e.g., Pending, In Progress, Completed) to find specific requests.
   - **Filter by Priority**: Filter service requests based on their priority (Low, Medium, High) to identify urgent requests.
   - The list will be organized to provide clear visibility into each service request's current status and priority.

5. **Sort the List**:  
   You can sort the service requests list by either **Priority** or **Status** for better organization:
   - **Sort by Priority**: Click on the "Sort by Priority" button to sort the service requests in order of priority (Low to High or High to Low).
   - **Sort by Status**: Click on the "Sort by Status" button to organize the service requests based on their current status (Pending, In Progress, Completed).

6. **Visual Display**:  
   - To get a visual overview of your service requests, you can view a **Visual Display** by clicking the **"See Visual Display"** button.
   - This will show a graphical representation of your service requests' status distribution across different categories.
   - **Category Filter**: On the **Visual Display** page, you can select a category from the dropdown list and click **Search** to filter the visual representation by the selected category.


## Application Behavior

- **Data Storage**: The application uses a local database stored within the app folder for data storage. User information, including securely hashed passwords, is stored in this local database to ensure reliability and security.
- **User Registration and Password Management**: User passwords are securely hashed before storage.
- **Exiting the Application**: Users can log out or exit the application securely.

## Data Structures 

1. **DependencyGraph graph**:  
 
2. **MaxHeap maxHeap**:
3. **AVLTree avlTree**:  
   
## References

- Payload (2021). WPF C# Professional Modern Flat UI Tutorial. Available at: [YouTube](https://www.youtube.com/watch?v=PzP8mw7JUzI&t=1946s) (Accessed: 9 September 2024).
- CodeCraks (2023). How to Create a Modern Splash Screen in WPF using C#. Available at: [YouTube](https://www.youtube.com/watch?v=XM_I1y1mh7k&t=2s) (Accessed: 16 September 2024).
- Coding Under Pressure (2020). How to Let User Browse and Upload Files in WPF C#. Available at: [YouTube](https://www.youtube.com/watch?v=DKYssZ8JUx0) (Accessed: 16 September 2024).
- Jd’s Code Lab (2021). WPF C# | Drag & Drop to Upload File UI. Available at: [YouTube](https://www.youtube.com/watch?v=eEa_Fl3ZguA&t=2s) (Accessed: 16 September 2024).
- Jeyderht (2021). C# WPF - Modern Vertical Menu. Available at: [YouTube](https://www.youtube.com/watch?v=Et-QcvwKzY4&t=4s) (Accessed: 16 September 2024).
- Jeyderht (2021). Jeyderht/WPFMODERNVERTICALMENU. Available at: [GitHub](https://github.com/Jeyderht/WPFModernVerticalMenu) (Accessed: 16 September 2024).
- OpenAI (2024). ChatGPT (Version 4.0). [Large language model]. Available at: [OpenAI](https://chat.openai.com/).
- OpenAI (2024). Afrikaans Resource File Setup. Available at: [OpenAI](https://chat.openai.com/).
- OpenAI (2024). Complete Strings and Referencing. Available at: [OpenAI](https://chat.openai.com/).
- OpenAI (2024). Translate Resource Dictionary. Available at: [OpenAI](https://chat.openai.com/).
- OpenAI. (2024). PROG7312- LOGIC CONVO. Available at: https://chatgpt.com/share/670ec6c0-884c-800b-8e1d-eccda71c1fbc [Accessed: 15 October 2024].
- OpenAI. (2024). Translate. Available at: https://chatgpt.com/share/670ec683-7ca0-800b-9699-8525ed348388 [Accessed: 15 October 2024].
- OpenAI. (2024). Custom Scroll Bar. Available at: https://chatgpt.com/share/670ec719-e504-800b-811b-44fe386c95d9 [Accessed: 15 October 2024].
- OpenAI. (2024). Dummy Data. Available at: https://chatgpt.com/share/670ec6c0-884c-800b-8e1d-eccda71c1fbc [Accessed: 15 October 2024].
- JD’s Code Lab (2020) WPF C# | Online Education Dashboard UI | UI Design in Wpf C# (Jd’s Code Lab), YouTube. Available at: https://www.youtube.com/watch?v=JrjBb9VC5Yk (Accessed: 15 October 2024).
- JD's Code Lab (2020) WPF C# | E-Course Dashboard UI | WPF UI Designs C# (Jd’s Code Lab), YouTube. Available at: https://www.youtube.com/watch?v=JrjBb9VC5Yk (Accessed: 15 October 2024).


## License

The MIT License (MIT)

Copyright (c) 2024 Hannah Michaelson

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

