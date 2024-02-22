
# iPractice Technical Assignment - Skeleton Project
 
 ## Get Started
 Please delete the contents in the migration folder, the existing DB, and run the `dotnet ef migrations add` and `dotnet ef database update` functions again to repopulate the migrations.

Open the project and click `Start` to host the server on	`localhost`.

 ## Features Completed
 - Adding psychologists availability, where the psychologists can input their availability at once in large blocks. This will be broken down into discrete 30m intervals.
	 - Time slots are expected to be 30m multiples. From the input time range, full multiples of 30m slots from the begin time will be added, and rest of the time is ignored.
	 - Some error handling and exceptions are implemented.
 - Clients can add new bookings with a specific psychologist and timeslot.
	 - If the timeslot is unavailable or already booked for that psychologist, error is raised for unavailable psychologist.
 - Fetching availabilities using `clientId` and `psychologistId` are both implemented.

 ## To Do
 - Separation of concerns to move all DB access functions to a different layer from the service layers, added to the DataAccess project. 
 - Separation of DB queries from the Booking service to a separate layer in the DataAccessProject.
