import React, { useState } from 'react';
import './SmsForm.css'; // Import CSS file

interface SmsFormProps {
  apiUrl: string; // API endpoint to send SMS
}

const SmsForm: React.FC<SmsFormProps> = ({ apiUrl }) => {
  const [phoneNumber, setPhoneNumber] = useState('');
  const [message, setMessage] = useState('');
  const [phoneNumberValid, setPhoneNumberValid] = useState(true);
  const [messageValid, setMessageValid] = useState(true);
  const [submitting, setSubmitting] = useState(false);

  const handlePhoneNumberChange = (event) => {
    const { value } = event.target;
    setPhoneNumber(value);
    setPhoneNumberValid(/^\+1\d{10}$/.test(value));
  };

  const handleMessageChange = (event) => {
    const { value } = event.target;
    setMessage(value);
    setMessageValid(value.length > 0);
  };

  const handleSubmit = async (event) => {
    event.preventDefault();

    if (phoneNumberValid && messageValid) {
      // Form is valid, make API call
      setSubmitting(true);

      try {
        const response = await fetch(apiUrl, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
         body: JSON.stringify({ phoneNumber,message  }),
        });

        if (!response.ok) {
          throw new Error('Failed to call API');
        }

        // Reset form state after successful API call
        setPhoneNumber('');
        setMessage('');
        setPhoneNumberValid(true);
        setMessageValid(true);
      } catch (error) {
        console.error('API Error:', error);
      } finally {
        setSubmitting(false);
      }
    } else {
      console.log('Form validation failed');
    }
  };

  return (
    <div className="sms-form">
    <form onSubmit={handleSubmit}>
      <div>
        <label htmlFor="phoneNumber">Phone Number:</label>
        <input
          type="text"
          id="phoneNumber"
          value={phoneNumber}
          onChange={handlePhoneNumberChange}
          style={{ borderColor: phoneNumberValid ? 'initial' : 'red' }}
        />
        {!phoneNumberValid && (
          <p style={{ color: 'red' }}>Please enter a valid phone number starting with "+1" followed by 10 digits.</p>
        )}
      </div>
      <div>
        <label htmlFor="message">Message:</label>
        <textarea
          id="message"
          value={message}
          onChange={handleMessageChange}
          style={{ borderColor: messageValid ? 'initial' : 'red' }}
        />
        {!messageValid && <p style={{ color: 'red' }}>Message is required.</p>}
      </div>
      <button type="submit">Submit</button>
    </form>
  </div>
);
};

export default SmsForm;
