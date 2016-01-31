import { connect } from 'react-redux'
import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

class View extends React.Component {

  render() {
    const { props: { viewData, params, location: { query, }, }, } = this
    return(
      <div>
        <Helmet title="Create login" />
        <h2>Create login.</h2>
        <h4>Choose a username and password.</h4>
        <hr />
        <Form onSubmit={onSubmit}
              initialValues={{ ...params, ...query }}
              viewData={viewData}
        />
      </div>
    )
  }
}

const select = state => ({
  viewData: state.app.register.createLogin.viewData,
})
export default connect(select)(View)
