const jwt = require('jsonwebtoken');

const token = 'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZEBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiQUQiLCJleHAiOjE3MTExMTA4NzF9.KBnOTSFxMmQtZ0tpkyDQbZZBZNKFPciziElnrzCm1gE'; // Replace this with your JWT token

// Verify and decode the JWT token
jwt.verify(token, 'ThisIsASecretKeyWithAtLeast16Bytes', (err, decoded) => {
    if (err) {
        console.error('JWT verification failed:', err);
    } else {
        // Debugging: Print decoded token
        console.log('Decoded token:', decoded);
        
        // Extract email and name from the decoded token
        const email = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'];;
        const name = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];;

        console.log('Email:', email);
        console.log('Name:', name);
    }
});