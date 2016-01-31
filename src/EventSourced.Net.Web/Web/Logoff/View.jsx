import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import onSubmit from './actions'
import Form from './Form'

class View extends Component {
  render() {
    const { username } = this.props
    return(
      <Form formKey="logoff" onSubmit={onSubmit} username={username} />
    )
  }
}

const select = state => ({
  username: state.app.login.username,
})
export default connect(select)(View)
