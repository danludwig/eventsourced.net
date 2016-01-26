import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import { submitLogin } from './actions'
import LoginForm from './LoginForm'

class Login extends Component {
  render() {
    const { props: { submitLogin, params, location: { query, }, }, } = this
    return(
      <LoginForm formKey="login" onSubmit={submitLogin} initialValues={{ ...params, ...query }} />
    )
  }
}

const select = () => ({})
const actions = { submitLogin }
const LoginConnector = connect(select, actions)(Login)

export default LoginConnector
