// Navbar.js

import { Link, NavLink } from 'react-router-dom';
import './S3FileList.css'; // Import CSS file
const Navbar = () => {
  return (
    <nav>
      <ul className="test">
        <li className='test2'>
          <NavLink to="/">Send Message</NavLink>
        </li>
        <li>
          <NavLink to="/S3FileList">Messages</NavLink>
        </li>
      </ul>
    </nav>
  );
};

export default Navbar;
