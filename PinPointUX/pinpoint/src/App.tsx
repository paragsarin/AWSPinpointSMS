import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import SmsForm from './SmsForm';
import S3FileList from './S3FileList';
import SomeComponent from './SomeComponent';
import Home from './Home';
import './App.css'
import { Route,  BrowserRouter as Router, Routes, useNavigate } from 'react-router-dom';
import Navbar from './navbar';

function App() {
 
  return (
    <>
    <header>
      <Navbar></Navbar>
    </header>
<Routes>
<Route path="/" element={<Home/>} />
<Route path="/S3FileList" element={<S3FileList/>} />
<Route path="*" element={<Home/>} />
</Routes>
   <div>
  
    </div>
    </>
  );
}

export default App
