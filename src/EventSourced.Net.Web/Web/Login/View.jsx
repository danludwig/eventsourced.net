import { connect } from 'react-redux'
import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

class View extends React.Component {
  render() {
    const { props: { params, location: { query, }, }, } = this
    return(
      <div>
        <Helmet title="Log in" />
        <h2>Log in.</h2>
        <h4>Use a local account to log in.</h4>
        <hr />
        <Form formKey="login" onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
      </div>
    )
  }
}

const select = () => ({})
export default connect(select)(View)
