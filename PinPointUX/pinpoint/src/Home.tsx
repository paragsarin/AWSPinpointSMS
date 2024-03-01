
import SmsForm from './SmsForm';
import './App.css'


function Home() {
 const  apiUrl="http://<URL>/SendSMS";

  return (
   

    <div>
    <h1>Send SMS</h1>
    <SmsForm apiUrl={apiUrl} />
  </div>
   
  );
}

export default Home
