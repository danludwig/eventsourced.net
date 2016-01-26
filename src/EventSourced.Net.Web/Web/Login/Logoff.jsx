import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import { submitLogoff } from './actions'
import LogoffForm from './LogoffForm'

class Logoff extends Component {
  render() {
    const { submitLogoff, username } = this.props
    return(
      <LogoffForm formKey="logoff" onSubmit={submitLogoff} username={username} />
    )
  }
}

const select = state => ({
  username: state.app.login.username,
})
const actions = { submitLogoff }
const LogoffConnector = connect(select, actions)(Logoff)

export default LogoffConnector
