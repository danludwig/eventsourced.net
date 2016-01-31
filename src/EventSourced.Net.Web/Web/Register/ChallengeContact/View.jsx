import { connect } from 'react-redux'
import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

class View extends React.Component {
  render() {
    const { props: { params, location: { query, }, }, } = this
    return(
      <div>
        <Helmet title="Register" />
        <h2>Register.</h2>
        <h4>Create a new account.</h4>
        <hr />
        <Form formKey="register" onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
      </div>
    )
  }
}

const select = () => ({})
export default connect(select)(View)
