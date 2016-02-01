import { Link } from 'react-router'
import { connect } from 'react-redux'
import onSubmitLogoff from './actions'
import LogoffForm from './Form'

const LoginNav = ({ username }) => (
  <div className="navbar-right">
    { username ?
    <LogoffForm onSubmit={onSubmitLogoff} username={username} />
    :
    <ul className="nav navbar-nav">
      <li><Link to="/register">Register</Link></li>
      <li><Link to="/login">Log in</Link></li>
    </ul>
    }
  </div>
)

LoginNav.propTypes = {
  username: React.PropTypes.string
}

const select = state => ({
  username: state.app.login.username,
})

export default connect(select)(LoginNav)
