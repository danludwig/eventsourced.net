import { connect } from 'react-redux'
import onSubmit from './actions'
import Form from './Form'

class View extends React.Component {
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
