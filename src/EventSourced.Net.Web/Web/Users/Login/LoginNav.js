import React, { Component, PropTypes } from 'react'
import { Link } from 'react-router'
import { connect } from 'react-redux'

class LoginNav extends Component {
  render() {
    const { username } = this.props
    return (
      <div className="navbar-right">
        { this.props.username ?
        <form action="/logoff" method="post">
          <ul className="nav navbar-nav navbar-right">
            <li>
              <a href="#/not-implemented" title="Manage">Hello {username}!</a>
            </li>
            <li>
              <button type="submit" className="btn btn-link navbar-btn navbar-link">Log off</button>
            </li>
          </ul>
        </form>
        :
        <ul className="nav navbar-nav">
          <li><Link to="/register">Register</Link></li>
          <li><Link to="/login">Log in</Link></li>
        </ul>
        }
      </div>
    )
  }

  static get propTypes() {
    return {
      username: PropTypes.string
    }
  }
}

function select(state) {
  return {
    username: state.app.data.user.username
  }
}

export default connect(select)(LoginNav)
