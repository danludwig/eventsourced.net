import { Link } from 'react-router'
import LogoffNav from '../Logoff/Nav'

export default () => (
  <div className="navbar navbar-inverse navbar-fixed-top">
    <div className="container">
      <div className="navbar-header">
        <button type="button" className="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
          <span className="sr-only">Toggle navigation</span>
          <span className="icon-bar"></span>
          <span className="icon-bar"></span>
          <span className="icon-bar"></span>
        </button>
        <Link to="/" className="navbar-brand">EventSourced.Net</Link>
      </div>
      <div className="navbar-collapse collapse">
        <ul className="nav navbar-nav">
          <li><Link to="/">Home</Link></li>
          <li><Link to="/about">About</Link></li>
          <li><Link to="/contact">Contact</Link></li>
        </ul>
        <LogoffNav />
      </div>
    </div>
  </div>
)
