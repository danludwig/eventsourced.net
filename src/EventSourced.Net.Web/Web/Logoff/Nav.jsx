import { Link } from 'react-router'
import { connect } from 'react-redux'
import Logoff from './View'

class LoginNav extends React.Component {
  static propTypes = {
    username: React.PropTypes.string
  };

  render() {
    const { username } = this.props
    return (
      <div className="navbar-right">
        { username ?
        <Logoff />
        :
        <ul className="nav navbar-nav">
          <li><Link to="/register">Register</Link></li>
          <li><Link to="/login">Log in</Link></li>
        </ul>
        }
      </div>
    )
  }
}

const select = state => ({
  username: state.app.login.username,
})

export default connect(select)(LoginNav)
